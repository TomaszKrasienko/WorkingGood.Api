using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Models.Offer;

namespace Application.Common.Mapper;

public class OfferMapperProfile : Profile
{
    public OfferMapperProfile()
    {
        CreateMapOfferGetOfferVm();
    }
    private void CreateMapOfferGetOfferVm()
    {
        CreateMap<Offer, GetOfferVM>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(
                    src => src.Id))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(
                    src => src.Content.Title))
            .ForMember(dest => dest.PositionType,
                opt => opt.MapFrom(
                    src => src.Position.Type))
            .ForMember(dest => dest.SalaryRangeMin,
                opt => opt.MapFrom(
                    src => src.SalaryRanges.ValueMin))
            .ForMember(dest => dest.SalaryRangeMax,
                opt => opt.MapFrom(
                    src => src.SalaryRanges.ValueMax))
            .ForMember(dest => dest.Description,
                opt => opt.MapFrom(
                    src => src.Content.Description))
            .ForMember(dest => dest.IsActive, 
                opt => opt.MapFrom(
                    src => src.OfferStatus.IsActive));
        
    }
}