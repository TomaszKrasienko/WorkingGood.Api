using System;
using Domain.Models.Offer;
using FluentAssertions;

namespace Domain.Tests.Models.Tests
{
	public class OfferTest
	{
		[Fact]
		public void ChangeStatus_Always_ShouldChangeIsActiveField()
		{
			//Arrange
			bool isActive = false;
			Offer offer = new Offer(
				"testTitle",
				"testPositionType",
				1000,
				1000,
				"descriptionTestdescriptionTestdescriptionTest",
				Guid.NewGuid(),
				isActive
				);
			//Act
			offer.ChangeStatus();
			//Assert
			offer.IsActive.Should().Be(!isActive);
		}
	}
}

