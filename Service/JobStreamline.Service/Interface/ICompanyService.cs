using System;
using JobStreamline.Entity;

namespace JobStreamline.Service;

public interface ICompanyService:IService<Company>
{
    OutputCompanyDto? Get(Guid Id);
    OutputCompanyDto Create(InputCompanyDto InputCompanyDto);
    OutputCompanyDto Update(Guid Id,InputCompanyDto InputCompanyDto);
}
