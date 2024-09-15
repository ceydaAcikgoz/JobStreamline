using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobStreamline.Entity.Enum;

namespace JobStreamline.Entity;


public class Job
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string Position { get; set; }
    public string Description { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int QualityScore { get; set; }
    public string Benefits { get; set; }
    public WorkType WorkType { get; set; }
    public string Salary { get; set; }
    public Currency Currency { get; set; }
    public string Location { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Active;
    public Company Company { get; set; }
    public string Qualifications { get; set; }
    public DateTime CreatedDate { get; set; }
}
