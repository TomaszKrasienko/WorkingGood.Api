using Application.CQRS.Offers.Queries.GetAllForCompany;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Offer;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;

namespace Application.Tests.CQRS.Offers.Queries.GetAllForCompany;

public class GetAllForCompanyQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllForCompanyQueryHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    public GetAllForCompanyQueryHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();        _
        _mockUnitOfWork.Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
    }

    [Fact]
    public async Task Handle_ForValidGetAllForCompanyQueryAndExistsOffer_ShouldReturnOkObjectResult()
    {
        _mockOfferRepository.Setup(x => x.GetAllAsync())
        IValidator<GetAllForCompanyQuery> _validator = new GetAllForEmployeeValidator();
        GetAllForCompanyQueryHandler getAllForCompanyQueryHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, _validator);
        
    }
}