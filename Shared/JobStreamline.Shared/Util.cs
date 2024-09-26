using JobStreamline.Entity;
using JobStreamline.Entity.Enum;

namespace JobStreamline.Shared;
public static class Util
{
    public static void CalculateScore(Job Job, List<string> Blackwords)
    {
        Job.QualityScore = 0;
        if (!string.IsNullOrEmpty(Job.Salary))
        {
            Job.QualityScore++;
        }
        if (Job.WorkType != WorkType.None)
        {
            Job.QualityScore++;
        }
        if (!string.IsNullOrEmpty(Job.Benefits))
        {
            Job.QualityScore++;
        }
        if (!string.IsNullOrEmpty(Job.Description))
        {
            bool isClear = !Blackwords.Any(s => Job.Description.ToUpper().Contains(s.ToUpper()));
            if (isClear)
            {
                Job.QualityScore += 2;
            }
        }
    }

    public static void CalculateCompanySize(Company Company)
    {
        if (Company.NumberOfEmployees is > 0 and <= 200)
        {
            Company.CompanySize = CompanySize.Small;
        }
        else if (Company.NumberOfEmployees is > 200 and < 1000)
        {
            Company.CompanySize = CompanySize.Medium;
        }
        else if (Company.NumberOfEmployees is > 1000 )
        {
            Company.CompanySize = CompanySize.Large;
        }
    }

}