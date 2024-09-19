using System;
using System.ComponentModel.DataAnnotations;
using JobStreamline.Entity.Enum;

namespace JobStreamline.Entity;


public class InputCompanyDto
{
    [Required(ErrorMessage = "PhoneNumber is required.")]
    [MaxLength(15, ErrorMessage = "PhoneNumber cannot exceed 15 characters.")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "CompanyName is required.")]
    [MaxLength(100, ErrorMessage = "CompanyName cannot exceed 100 characters.")]
    public string CompanyName { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    [MaxLength(100, ErrorMessage = "Address cannot exceed 100 characters.")]
    public string Address { get; set; }

    [MaxLength(50, ErrorMessage = "ContactEmail cannot exceed 50 characters.")]
    public string ContactEmail { get; set; }

    [Required(ErrorMessage = "NumberOfEmployees is required.")]
    public int NumberOfEmployees { get; set; } 

}