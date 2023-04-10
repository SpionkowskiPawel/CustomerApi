using Bogus;
using Customer.Bl.Commands.RemoveCustomer;
using Customer.Bl.Validators;
using FluentAssertions;
using NUnit.Framework;

namespace Consumer.UnitTest.Validators
{
    public class RemoveCustomerCommandRequestValidatorTests
    {
        [Test]
        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(-1231)]
        public void RemoveCustomer_WhenCustomerIdIsNegativeOrZero_ThenThrowException(int incorrectCustomerId)
        {
            //Arrange
            RemoveCustomerCommandRequestValidator validator = new();
            RemoveCustomerCommandRequest request = new Faker<RemoveCustomerCommandRequest>()
                .RuleFor(u => u.CustomerId, (f, u) => incorrectCustomerId);

            //Act
            FluentValidation.Results.ValidationResult result = validator.Validate(request);

            //Assert
            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"'Customer Id' must be greater than or equal to '1'.");
            result.Errors.First().PropertyName.Should().Be($"CustomerId");
        }
        
        [Test]
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(1231)]
        public void AddCustomer_WhenDataIsCorrect_ThenReturnSuccess(int correctCustomerId)
        {
            //Arrange
            RemoveCustomerCommandRequestValidator validator = new();
            RemoveCustomerCommandRequest request = new Faker<RemoveCustomerCommandRequest>()
                .RuleFor(u => u.CustomerId, (f, u) => correctCustomerId);

            //Act
            FluentValidation.Results.ValidationResult result = validator.Validate(request);

            //Assert
            result.Errors.Should().HaveCount(0);
            result.IsValid.Should().BeTrue();
        }
    }
}
