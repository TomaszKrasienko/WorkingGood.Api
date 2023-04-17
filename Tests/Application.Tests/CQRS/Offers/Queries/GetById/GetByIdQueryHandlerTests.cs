using System;
using Application.CQRS.Offers.Queries.GetById;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.Offers.Queries.GetById
{
	public class GetByIdQueryHandlerTests
	{
		private readonly Mock<ILogger<GetByIdQueryHandler>> _mockLogger;
		private readonly Mock<IUnitOfWork> _mockUnitOfWork;
		private readonly Mock<IOfferRepository> _mockOffersRepository;
		public GetByIdQueryHandlerTests()
		{
			_mockLogger = new();
			_mockUnitOfWork = new();
			_mockOffersRepository = new();
			_mockUnitOfWork.Setup(x => x.OffersRepository)
				.Returns(_mockOffersRepository.Object);
		}
		[Fact]
		public async Task Handle_ForInvalidGetByIdQuery_ShouldReturnBaseMessageWithOffer()
		{
			//Arrange
			Offer offer = new(
				"test",
				"test",
				10000,
				12000,
				"test descritpion test descrption",
				Guid.NewGuid(),
				true);
			GetByIdQuery getByIdQuery = new()
			{
				Id = offer.Id
			};
			_mockOffersRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(offer);
			IValidator<GetByIdQuery> validator = new GetByIdValidator();
			var handler = new GetByIdQueryHandler(
				_mockLogger.Object,
				_mockUnitOfWork.Object,
				validator);
			//Act
			var result = await handler.Handle(getByIdQuery, new CancellationToken());
			//Assert
			result.Should().BeOfType<BaseMessageDto>();
			result.Object.Should().BeOfType<Offer>();
			result.Object.Should().BeEquivalentTo(offer);
			result.Errors.Should().BeNull();
		}
	}
}

