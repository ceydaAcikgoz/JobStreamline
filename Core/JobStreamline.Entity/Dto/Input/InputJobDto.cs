using System;
using System.ComponentModel.DataAnnotations;
using JobStreamline.Entity.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JobStreamline.Entity;

public class InputJobDTO
{

    [Required(ErrorMessage = "CompanyId is required.")]
    public Guid CompanyId { get; set; }

    [Required(ErrorMessage = "Position is required.")]
    [MaxLength(100, ErrorMessage = "Position cannot exceed 100 characters.")]
    public string Position { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }

    [MaxLength(500, ErrorMessage = "Benefits cannot exceed 500 characters.")]
    public string Benefits { get; set; }

    [EnumDataType(typeof(WorkType))]
    [JsonConverter(typeof(StringEnumConverter))]
    public WorkType WorkType { get; set; }

    public double Salary { get; set; }

    [EnumDataType(typeof(Currency))]
    [JsonConverter(typeof(StringEnumConverter))]
    public Currency Currency { get; set; }

    [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
    public string Location { get; set; }

    [MaxLength(500, ErrorMessage = "Qualifications cannot exceed 500 characters.")]
    public string Qualifications { get; set; }

    public DateTime CreatedDate { get; set; }

    [EnumDataType(typeof(JobStatus))]
    [JsonConverter(typeof(StringEnumConverter))]
    public JobStatus Status { get; set; }
}
