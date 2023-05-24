using Application.Common.Mapper;
using Application.ViewModels.GetEmployee;
using AutoMapper;
using Domain.Models.Employee;
using FluentAssertions;

namespace Application.Tests.Mappers;

public class EmployeeMapperTests
{
    [Fact]
    public void  GetEmployeeVMMap_ForNonActiveEmployee_ShouldReturnGetEmployeeVmWithNonActiveStatus()
    {
        //Arrange
        string firstName = "testFirstName";
        string lastName = "testLastName";
        string email = "test@test.pl";
        var config = new MapperConfiguration(cfg => cfg.AddProfile<EmployeeMapperProfile>());
        var mapper = config.CreateMapper();
        Employee employee = new Employee(firstName, lastName, email, "passwordTest123!", Guid.NewGuid());
        //Act
        GetEmployeeVM getEmployeeVm = mapper.Map<GetEmployeeVM>(employee);
        //Assert
        getEmployeeVm.FirstName.Should().Be(firstName);
        getEmployeeVm.LastName.Should().Be(lastName);
        getEmployeeVm.Email.Should().Be(email);
        getEmployeeVm.IsActive.Should().BeFalse();
    }
}