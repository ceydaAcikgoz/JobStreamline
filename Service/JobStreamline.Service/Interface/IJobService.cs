

using System;
using JobStreamline.Entity;

namespace JobStreamline.Service;

public interface IJobService : IService<Job>
{
    OutputJobDto? Get(Guid Id);
    Task<OutputJobDto> Create(InputJobDTO InputJobDTO);
    Task<OutputJobDto> Update(InputJobDTO InputJobDTO);
    Task CreateIndex();
    Task DeleteJobAsync(string id);
    Task<List<JobElasticDTO>> SearchJobs(string? text);
    Task BulkDeleteDocumentsAsync(List<string> ids);

}
