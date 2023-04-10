using Bogus;
using Customer.Bl.Commands.AddCustomer;
using Customer.Bl.Validators;
using FluentAssertions;
using NUnit.Framework;

namespace Consumer.UnitTest.Validators
{
    public class AddCustomerCommandRequestValidatorTests
    {
        [Test]
        public void AddCustomer_WhenFirstNameIsEmpty_ThenThrowException()
        {
            //Arrange
            AddCustomerCommandRequestValidator validator = new();
            AddCustomerCommandRequest request = new Faker<AddCustomerCommandRequest>()
                .RuleFor(u => u.FirstName, (f, u) => string.Empty)
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName());

            //Act
            FluentValidation.Results.ValidationResult result = validator.Validate(request);

            //Assert
            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"'First Name' must not be empty.");
            result.Errors.First().PropertyName.Should().Be($"FirstName");
        }

        [Test]
        public void AddCustomer_WhenLastNameIsEmpty_ThenThrowException()
        {
            //Arrange
            AddCustomerCommandRequestValidator validator = new();
            AddCustomerCommandRequest request = new Faker<AddCustomerCommandRequest>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => string.Empty);

            //Act
            FluentValidation.Results.ValidationResult result = validator.Validate(request);

            //Assert
            result.Errors.Should().HaveCount(1);
            result.Errors.First().ErrorMessage.Should().Be($"'Last Name' must not be empty.");
            result.Errors.First().PropertyName.Should().Be($"LastName");
        }

        [Test]
        public void AddCustomer_WhenDataIsCorrect_ThenReturnSuccess()
        {
            //Arrange
            AddCustomerCommandRequestValidator validator = new();
            AddCustomerCommandRequest request = new Faker<AddCustomerCommandRequest>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName());

            //Act
            FluentValidation.Results.ValidationResult result = validator.Validate(request);

            //Assert
            result.Errors.Should().HaveCount(0);
            result.IsValid.Should().BeTrue();
        }
    }
}
