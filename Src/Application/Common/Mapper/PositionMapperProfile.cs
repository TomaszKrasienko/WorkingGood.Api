using Application.DTOs.Offers;
using AutoMapper;
using Domain.Models.Offer;

namespace Application.Common.Mapper;

public class PositionMapperProfile : Profile
{
    public PositionMapperProfile()
    {
        CreateMap<Position, GetPositionDto>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(
                    src => src.Type));
    }
}