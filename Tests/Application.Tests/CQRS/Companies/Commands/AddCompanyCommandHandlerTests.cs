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
    private readonly Mock<ILogger<AddCompanyCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICompanyRepository> _mockCompanyRepository;
    private Mock<ICompanyChecker> _mockCompanyChecker;
    public AddCompanyCommandHandlerTests()
    {
        _mockLogger = new Mock<ILogger<AddCompanyCommandHandler>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCompanyRepository = new Mock<ICompanyRepository>();
        _mockUnitOfWork.Setup(x => x.CompanyRepository).Returns(_mockCompanyRepository.Object);
        _mockCompanyChecker = new Mock<ICompanyChecker>();
    }
    [Fact]
    public async Task Handle_ForValidAddCompanyCommand_ShouldReturnBaseMessageDtoWithMessageAndCompanyObject()
    {
        //Arrange
            IValidator<AddCompanyCommand> validator = new AddCompanyValidator(_mockCompanyChecker.Object);
            AddCompanyCommand addCompanyCommand = new AddCompanyCommand()
            {
                CompanyDto = new()
                {
                    Name = "TestCompany"
                }
            };
            var addCompanyCommandHandler = new AddCompanyCommandHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
            var result = await addCompanyCommandHandler.Handle(addCompanyCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Added company successfully");
        result.Object.Should().BeOfType<Company>();
        result.Errors.Should().BeNull();
    }
    [Fact]
    public async Task Handle_ForInvalidAddCompanyCommand_ShouldReturnErrorBaseMessage()
    {
        //Arrange
        IValidator<AddCompanyCommand> validator = new AddCompanyValidator(_mockCompanyChecker.Object);
        AddCompanyCommand addCompanyCommand = new AddCompanyCommand()
        {
            CompanyDto = new()
            {
                Name = ""
            }
        };
        var addCompanyCommandHandler = new AddCompanyCommandHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
        var result = await addCompanyCommandHandler.Handle(addCompanyCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Bad Request");
        result.Errors.Should().BeOfType<List<string>>();
        result.Object.Should().BeNull();
    }   
    [Fact]
    public async Task Handle_ForExistingCompanyNameInAddCompanyCommand_ShouldReturnErrorBaseMessage()
    {
        //Arrange
        _mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<string>())).Returns(true);
        IValidator<AddCompanyCommand> validator = new AddCompanyValidator(_mockCompanyChecker.Object);
        AddCompanyCommand addCompanyCommand = new AddCompanyCommand()
        {
            CompanyDto = new()
            {
                Name = "TestCompany"
            }
        };
        var addCompanyCommandHandler = new AddCompanyCommandHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
        var result = await addCompanyCommandHandler.Handle(addCompanyCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Bad Request");
        result.Errors.Should().BeOfType<List<string>>();
        result.Object.Should().BeNull();
    }
}