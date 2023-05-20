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
			offer.OfferStatus.IsActive.Should().Be(!isActive);
		}

		[Fact]
		public void EditOffer_ForArguments_ShouldEditOffer()
		{
			//Arrange
			Offer offer = new Offer(
				"testTitle",
				"testPositionType",
				1000,
				1000,
				"descriptionTestdescriptionTestdescriptionTest",
				Guid.NewGuid(),
				true
			);
			string newTitle = "newTestTitle";
			double newSalaryRangeMin = 12000;
			double newSalaryRangeMax = 12000;
			string newDescription = "newDescriptionnewDescriptionnewDescriptionnewDescription";
			bool newIsActive = false;
			//Act
			offer.EditOffer(newTitle, newSalaryRangeMin, newSalaryRangeMax, newDescription, newIsActive);
			//Assert
			offer.Content.Title.Should().Be(newTitle);
			offer.Content.Description.Should().Be(newDescription);
			offer.OfferStatus.IsActive.Should().Be(newIsActive);
			offer.SalaryRanges.ValueMin.Should().Be(newSalaryRangeMin);
			offer.SalaryRanges.ValueMax.Should().Be(newSalaryRangeMax);
		}
	}
}

