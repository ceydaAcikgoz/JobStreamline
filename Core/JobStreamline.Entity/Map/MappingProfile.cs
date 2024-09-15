using AutoMapper;

namespace JobStreamline.Entity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, InputCompanyDto>()
            .ForMember(dest => dest.CompanySize, opt => opt.MapFrom(src => src.CompanySize));

        CreateMap<InputCompanyDto, Company>();

        CreateMap<Company, OutputCompanyDto>()
            .ForMember(dest => dest.CompanySize, opt => opt.MapFrom(src => src.CompanySize));

        CreateMap<OutputCompanyDto, Company>();

        CreateMap<Job, InputJobDTO>()
            .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
            .ForMember(dest => dest.WorkType, opt => opt.MapFrom(src => src.WorkType));

        CreateMap<InputJobDTO, Job>();

        CreateMap<Job, OutputJobDto>()
            .ForMember(dest => dest.Salary, opt => opt.MapFrom(src => src.Salary))
            .ForMember(dest => dest.WorkType, opt => opt.MapFrom(src => src.WorkType));

        CreateMap<OutputJobDto, Job>();
    }
}