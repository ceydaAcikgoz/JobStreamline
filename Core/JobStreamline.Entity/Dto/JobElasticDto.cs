using System;
namespace JobStreamline.Entity;
public class JobElasticDTO
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Position { get; set; }
    public string Description { get; set; }
    public int QualityScore { get; set; }
    public string Benefits { get; set; }
    public string WorkType { get; set; }
    public string Salary { get; set; }
    public string Currency { get; set; }
    public string Location { get; set; }
    public string Qualifications { get; set; }
}