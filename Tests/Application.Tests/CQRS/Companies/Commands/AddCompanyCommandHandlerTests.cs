using Application.CQRS.Companies.Commands;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Company;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.Companies.Commands;

public class AddCompanyCommandHandlerTests
{
    public AddCompanyCommandHandlerTests()
    {
        
    }

    [Fact]
    public async Task Handle_ValidAddCompanyCommand_ShouldReturnBaseMessageDtoWithMessageAndCompanyObject()
    {
        //Arrange
            Mock<ILogger<AddCompanyCommandHandler>> mockLogger = new Mock<ILogger<AddCompanyCommandHandler>>();
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            Mock<ICompanyRepository> mockCompanyRepository = new Mock<ICompanyRepository>();
            mockUnitOfWork.Setup(x => x.CompanyRepository).Returns(mockCompanyRepository.Object);
            Mock<ICompanyChecker> mockCompanyChecker = new Mock<ICompanyChecker>();
            mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<string>())).Returns(false);
            IValidator<AddCompanyCommand> validator = new AddCompanyValidator(mockCompanyChecker.Object);
            AddCompanyCommand addCompanyCommand = new AddCompanyCommand()
            {
                CompanyDto = new()
                {
                    Name = "TestCompany"
                }
            };
            var addCompanyCommandHandler = new AddCompanyCommandHandler(mockLogger.Object, mockUnitOfWork.Object, validator);
        //Act
            var result = await addCompanyCommandHandler.Handle(addCompanyCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Added company successfully");
        result.Object.Should().BeOfType<Company>();

    }
}