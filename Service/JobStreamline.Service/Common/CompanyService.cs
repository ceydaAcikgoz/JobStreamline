using System;
using AutoMapper;
using JobStreamline.DataAccess;
using JobStreamline.Entity;
using JobStreamline.Shared;

namespace JobStreamline.Service;

public class CompanyService : Service<Company>, ICompanyService
{
    public CompanyService(IMapper Mapper, IUnitOfWork UnitOfWork) : base(Mapper, UnitOfWork)
    {
    }

    public OutputCompanyDto Create(InputCompanyDto InputCompanyDto)
    {
        Company company = this._iMapper.Map<Company>(InputCompanyDto);
        Util.CalculateCompanySize(company);
        this.Create(company);
        return this._iMapper.Map<OutputCompanyDto>(company);
    }

    public OutputCompanyDto Update(InputCompanyDto InputCompanyDto)
    {
        Company company = this._iMapper.Map<Company>(InputCompanyDto);
        this.Update(company);
        return this._iMapper.Map<OutputCompanyDto>(company);
    }

    OutputCompanyDto? ICompanyService.Get(Guid Id)
    {
        Company company = this.Get(Id);
        return this._iMapper.Map<OutputCompanyDto>(company);
    }
}
