using System;
using JobStreamline.Entity.Enum;

namespace JobStreamline.Entity;

public class OutputJobDto
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Position { get; set; }
    public string Description { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int QualityScore { get; set; }
    public string Benefits { get; set; }
    public WorkType WorkType { get; set; }
    public string Salary { get; set; }
    public string Location { get; set; }

}
