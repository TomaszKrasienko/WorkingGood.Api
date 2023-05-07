using Application.ViewModels.GetEmployee;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Models.Employee;
using Domain.Models.Offer;

namespace Application.Common.Mapper;
public class EmployeeMapperProfile : Profile
{
    public EmployeeMapperProfile()
    {
        CreateMapEmployeeGetEmployeeVm();
    }
    private void CreateMapEmployeeGetEmployeeVm()
    {
        CreateMap<Employee, GetEmployeeVM>()
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src =>
                    src.EmployeeName.FirstName))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src =>
                    src.EmployeeName.LastName))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src =>
                    src.Email.EmailAddress))
            .ForMember(dest => dest.IsActive, 
                opt => opt.MapFrom(src =>
                    src.EmployeeStatus.IsActive));
    }

}