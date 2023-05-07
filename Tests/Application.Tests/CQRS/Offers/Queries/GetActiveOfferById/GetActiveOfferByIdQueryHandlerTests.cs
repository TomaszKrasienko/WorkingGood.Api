using System;
using Application.Common.Mapper;
using Application.CQRS.Offers.Queries.GetById;
using Application.DTOs;
using Application.ViewModels.GetEmployee;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.Offers.Queries.GetById
{
	public class GetActiveOfferByIdQueryHandlerTests
	{
		private readonly Mock<ILogger<GetActiveOfferByIdQueryHandler>> _mockLogger;
		private readonly Mock<IUnitOfWork> _mockUnitOfWork;
		private readonly Mock<IOfferRepository> _mockOffersRepository;
		private readonly Mock<IMapper> _mockMapper;
		private readonly Mock<IOfferChecker> _mockOfferChecker;
		public GetActiveOfferByIdQueryHandlerTests()
		{
			_mockLogger = new();
			_mockUnitOfWork = new();
			_mockOffersRepository = new();
			_mockUnitOfWork.Setup(x => x.OffersRepository)
				.Returns(_mockOffersRepository.Object);
			_mockMapper = new();
			_mockOfferChecker = new();
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
			GetOfferVM getOfferVm = new()
			{
				Id = offer.Id,
				Title = offer.Content.Title,
				PositionType = offer.Position.Type,
				SalaryRangeMax = offer.SalaryRanges.ValueMax,
				SalaryRangeMin = offer.SalaryRanges.ValueMin,
				IsActive = offer.OfferStatus.IsActive,
				Description = offer.Content.Description
			};
			GetActiveOfferByIdQuery getByIdQuery = new()
			{
				Id = offer.Id
			};
			_mockOffersRepository
				.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
				.ReturnsAsync(offer);
			_mockMapper
				.Setup(x => x.Map<GetOfferVM>(It.IsAny<Offer>()))
				.Returns(getOfferVm);
			_mockOfferChecker
				.Setup(x => x.IsOfferActive(It.IsAny<Guid>()))
				.Returns(true);
			_mockOfferChecker
				.Setup(x => x.IsOfferActive(It.IsAny<Guid>()))
				.Returns(true);
			IValidator<GetActiveOfferByIdQuery> validator = new GetActiveOfferByIdValidator(_mockOfferChecker.Object);
			var handler = new GetActiveOfferByIdQueryHandler(
				_mockLogger.Object,
				_mockUnitOfWork.Object,
				validator,
				_mockMapper.Object);
			//Act
			var result = await handler.Handle(getByIdQuery, new CancellationToken());
			//Assert
			result.Should().BeOfType<BaseMessageDto>();
			result.Object.Should().BeOfType<GetOfferVM>();
			result.Object.Should().BeEquivalentTo(getOfferVm);
			result.Errors.Should().BeNull();
		}
	}
}

