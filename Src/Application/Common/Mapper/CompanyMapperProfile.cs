using Application.ViewModels.Company;
using AutoMapper;
using Domain.Models.Company;

namespace Application.Common.Mapper;

public class CompanyMapperProfile : Profile
{
    public CompanyMapperProfile()
    {
        CreateMap<Company, CompanyVM>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(
                    src => src.Name.Name))
            .ForMember(dest => dest.Logo,
            opt => opt.MapFrom(
                src => src.Logo == null ? null : src.Logo.Logo));
    }
}