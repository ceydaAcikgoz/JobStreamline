using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JobStreamline.Entity.Enum;

namespace JobStreamline.Entity;

public class Company
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public int JobPostingLimit { get; set; }
    public string ContactEmail { get; set; }
    public virtual ICollection<Job> Jobs { get; set; }
    public int NumberOfEmployees { get; set; } 
    public CompanySize CompanySize { get; set; }

}


