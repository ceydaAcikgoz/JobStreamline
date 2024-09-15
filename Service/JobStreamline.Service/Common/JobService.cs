using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using JobStreamline.Shared;
using JobStreamline.DataAccess;
using JobStreamline.Entity;

namespace JobStreamline.Service;

public class JobService : Service<Job>, IJobService
{
    private ICompanyService _iCompanyService;
    private IBlackwordService _iBlackWordService;
    public JobService(ICompanyService CompanyService,IBlackwordService BlackWordService, IMapper Mapper, IUnitOfWork UnitOfWork) : base(Mapper, UnitOfWork)
    {
        _iCompanyService = CompanyService;
        _iBlackWordService= BlackWordService;
    }

    public OutputJobDto Create(InputJobDTO InputJobDTO)
    {
        Company company = _iCompanyService.GetAll(s => s.Id == InputJobDTO.CompanyId).SingleOrDefault();
        if (company?.JobPostingLimit > 0)
        {
            Job job = this._iMapper.Map<Job>(InputJobDTO);
            Util.CalculateScore(job, _iBlackWordService.AllBlackword().Result.Select(x => x.Value).ToList());
            this.Create(job);
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

    public OutputJobDto Update(InputJobDTO InputJobDTO)
    {
        Job job = this._iMapper.Map<Job>(InputJobDTO);
        this.Update(job);
        return this._iMapper.Map<OutputJobDto>(job);
    }

    OutputJobDto? IJobService.Get(Guid Id)
    {
        Job job = this.Get(Id);
        return this._iMapper.Map<OutputJobDto>(job);
    }
}
