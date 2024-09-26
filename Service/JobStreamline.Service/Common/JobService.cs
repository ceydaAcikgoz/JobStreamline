using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using JobStreamline.Shared;
using JobStreamline.DataAccess;
using JobStreamline.Entity;
using Microsoft.Extensions.Configuration;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Elastic.Clients.Elasticsearch;

namespace JobStreamline.Service;

public class JobService : Service<Job>, IJobService
{
    private ICompanyService _iCompanyService;
    private IBlackwordService _iBlackWordService;
    private IElasticsearchService _elasticsearchService;
    private string _indexName;
    public JobService(ICompanyService CompanyService, IBlackwordService BlackWordService, IMapper Mapper, IUnitOfWork UnitOfWork,
    IElasticsearchService ElasticsearchService, IConfiguration Configuration) : base(Mapper, UnitOfWork)
    {
        _iCompanyService = CompanyService;
        _iBlackWordService = BlackWordService;
        _elasticsearchService = ElasticsearchService;
        _indexName = Configuration["Elasticsearch:JobIndex"];
    }

    public async Task<OutputJobDto> Create(InputJobDTO InputJobDTO)
    {
        Company company = _iCompanyService.GetAll(s => s.Id == InputJobDTO.CompanyId).SingleOrDefault();
        if (company?.JobPostingLimit > 0)
        {
            Job job = this._iMapper.Map<Job>(InputJobDTO);
            Util.CalculateScore(job, _iBlackWordService.AllBlackword().Result.Select(x => x.Value).ToList());
            this.Create(job);
            await this.IndexJobAsync(this._iMapper.Map<JobElasticDTO>(job));
            OutputJobDto outputJobDto = this._iMapper.Map<OutputJobDto>(job);
            company.JobPostingLimit--;
            this._iCompanyService.Update(company);
            return outputJobDto;
        }
        else
        {
            throw new ValidationException($"{company.CompanyName} şirketinin ilan hakkı kalmamıştır.");
        }
    }

    public async Task<OutputJobDto> Update(Guid Id, InputJobDTO InputJobDTO)
    {
        Job job = this.Get(Id);
        if (job == null)
        {
            throw new ArgumentNullException("İlan bulunamadı.");
        }
        job = this._iMapper.Map<Job>(InputJobDTO);
        this.Update(job);
        await this.UpdateDocumentAsync(Id, this._iMapper.Map<JobElasticDTO>(job));
        OutputJobDto outputJobDto = this._iMapper.Map<OutputJobDto>(job);
        return outputJobDto;
    }

    OutputJobDto? IJobService.Get(Guid Id)
    {
        Job job = this.Get(Id);
        return this._iMapper.Map<OutputJobDto>(job);
    }

    public async Task CreateIndex()
    {
        var client = _elasticsearchService.Client;
        var isExists = await client.Indices.ExistsAsync(_indexName);

        if (!isExists.IsValidResponse)
        {
            var isCreateR = await client.Indices.CreateAsync(_indexName);

            if (!isCreateR.IsValidResponse)
            {
                throw new Exception(isExists.DebugInformation);
            }
        }
    }

    private async Task IndexJobAsync(JobElasticDTO job)
    {
        var client = _elasticsearchService.Client;
        var response = await client.IndexAsync(job, idx => idx.Index(_indexName));
        if (response.IsValidResponse)
        {
            Console.WriteLine("Job successfully indexed.");
        }
        else
        {
            Console.WriteLine($"Failed to index job: {response.DebugInformation}");
        }
    }

    private async Task UpdateDocumentAsync(Guid id, JobElasticDTO document)
    {
        var client = _elasticsearchService.Client;
        var response = await client.UpdateAsync<JobElasticDTO, JobElasticDTO>(id, u => u.Index(_indexName).Doc(document));

        if (response.IsValidResponse)
        {
            Console.WriteLine("Job successfully edited.");
        }
        else
        {
            Console.WriteLine($"Failed to edited job: {response.DebugInformation}");
        }
    }

    public async Task DeleteJobAsync(string id)
    {
        var client = _elasticsearchService.Client;
        var response = await client.DeleteAsync<Job>(id, idx => idx.Index(_indexName));
        if (response.IsValidResponse)
        {
            Console.WriteLine("Job successfully deleted.");
        }
        else
        {
            Console.WriteLine($"Failed to delete job: {response.DebugInformation}");
        }
    }

    public async Task BulkDeleteDocumentsAsync(List<string> ids)
    {
        var client = _elasticsearchService.Client;
        var bulkRequest = new BulkRequestDescriptor();

        foreach (var id in ids)
        {
            bulkRequest.Delete(d => d.Index(_indexName).Id(id));
        }

        var response = await client.BulkAsync(bulkRequest);

        if (!response.IsValidResponse)
        {
            Console.WriteLine("Some documents failed to delete.");
            // Daha fazla hata detayı için
            foreach (var itemWithError in response.ItemsWithErrors)
            {
                Console.WriteLine($"Failed to delete document {itemWithError.Id}: {itemWithError.Error.Reason}");
            }
        }
        else
        {
            Console.WriteLine("All documents deleted successfully.");
        }
    }

    public async Task<List<JobElasticDTO>> SearchJobs(string? text)
    {
        var client = _elasticsearchService.Client;

        var searchResponse = await client.SearchAsync<JobElasticDTO>(s => s
        .Index(_indexName)
        .Query(q => q
            .FunctionScore(fs => fs
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(text)
                        .Fields("description")
                        .Type(TextQueryType.BestFields) // ankara-akkara aramasında score göre ikisini de getir ancak score olarak doğru aramayı en önce getirir. json'ın
                        .Fuzziness(new Fuzziness(1)) // yanlış yazımlar için 1 hak veriyor. ankara-akkara gibi
                        .MinimumShouldMatch("50%") // aranan value'nun en az %50'inin eşleşmesi lazım.
                    )
                )
                .BoostMode(FunctionBoostMode.Multiply)
            )
        ));

        return [.. searchResponse.Documents];
    }
}
