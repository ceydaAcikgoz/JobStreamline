using System;
using JobStreamline.Entity.Enum;

namespace JobStreamline.Entity;

public class OutputCompanyDto
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public int JobPostingLimit { get; set; }
    public string ContactEmail { get; set; }

    public CompanySize CompanySize { get; set; }
}
