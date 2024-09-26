using JobStreamline.Entity;
using JobStreamline.Entity.Enum;

namespace JobStreamline.Test;

public static class TestUtil
{
    public static string RandomDigits(int length)
    {
        var random = new Random();
        string s = string.Empty;
        for (int i = 0; i < length; i++)
            s = String.Concat(s, random.Next(10).ToString());
        return s;
    }

    public static List<InputCompanyDto> GenerateCompanies()
    {
        var companies = new List<InputCompanyDto>();
        
        for (int i = 1; i <= 50; i++)
        {
            var companyDto = new InputCompanyDto
            {
                PhoneNumber = $"555-000{i:D3}",
                CompanyName = $"Company {i}",
                Address = $"Address {i}",
                ContactEmail = $"contact{i}@company{i}.com",
                NumberOfEmployees = 50 + i
            };

            companies.Add(companyDto);
        }

        return companies;
    }

    public static List<CompanyJobData> GenerateCompanyJobs(int count)
    {
        var companiesWithJobs = new List<CompanyJobData>();
        
        for (int i = 1; i <= count; i++)
        {
            var companyId = Guid.NewGuid();
            var companyDto = new CompanyJobData
            {
                Company = new InputCompanyDto
                {
                    PhoneNumber = RandomDigits(10),
                    CompanyName = $"Company {i}",
                    Address = $"Address {i}",
                    ContactEmail = $"contact{i}@company{i}.com",
                    NumberOfEmployees = 150 + i
                },
                Jobs = new List<InputJobDTO>
                {
                    new InputJobDTO
                    {
                        CompanyId = companyId,
                        Position = $"Software Developer {i}-1",
                        Description = $"Develop and maintain software applications for Company {i}.",
                        Benefits = "Health insurance, 401k",
                        WorkType = WorkType.FullTime,
                        Salary = 70000 + i * 1000,
                        Currency = Currency.USD,
                        Location = $"Location {i}-1",
                        Qualifications = "Bachelor's degree in Computer Science"
                    },
                    new InputJobDTO
                    {
                        CompanyId = companyId,
                        Position = $"Product Manager {i}-2",
                        Description = $"Manage product development and strategy for Company {i}.",
                        Benefits = "Health insurance, Stock options",
                        WorkType = WorkType.PartTime,
                        Salary = 80000 + i * 1000,
                        Currency = Currency.USD,
                        Location = $"Location {i}-2",
                        Qualifications = "MBA or related field"
                    }
                }
            };

            companiesWithJobs.Add(companyDto);
        }

        return companiesWithJobs;
    }
}

public class CompanyJobData
{
    public InputCompanyDto Company { get; set; }
    public List<InputJobDTO> Jobs { get; set; } = new List<InputJobDTO>();
}