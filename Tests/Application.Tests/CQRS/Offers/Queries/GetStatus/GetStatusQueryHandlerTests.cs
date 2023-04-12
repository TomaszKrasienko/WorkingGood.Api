using Application.CQRS.Offers.Queries.GetOfferStatus;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.Offers.Queries.GetStatus;

public class GetStatusQueryHandlerTests
{
    private readonly Mock<ILogger<GetStatusQueryHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    public GetStatusQueryHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockUnitOfWork.Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
    }
    
}