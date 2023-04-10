using Customer.Dal.Dto.Dtos;
using Customer.Dal.Interfaces.Repositories;
using Customer.Dal.StubRepositories;
using NUnit.Framework;
using FluentAssertions;

namespace Consumer.UnitTest.Repositories
{
    public class StubCustomerRepositoryTests
    {
        [Test]
        public async Task GetCustomer_WhenCustomersExists_ThenGetIt()
        {
            //Arrange
            ICustomerRepository stubCustomerRepository = new StubCustomerRepository();

            //Act
            IEnumerable<CustomerDto> CustomerDtos = await stubCustomerRepository.GetCustomersAsync();

            //Assert
            CustomerDtos.Should().NotBeNull();
            CustomerDtos.Should().HaveCount(3);
            CustomerDtos.First().Id.Should().Be(1);
            CustomerDtos.First().LastName.Should().Be("Tionge");
            CustomerDtos.First().FirstName.Should().Be("Max");
        }

        [Test]
        public async Task RemoveCustomer_WhenCustomerIdWasProvided_ThenReturnTrue()
        {
            //Arrange
            const int CustomerId = 1;
            ICustomerRepository stubCustomerRepository = new StubCustomerRepository();

            //Act
            bool result = await stubCustomerRepository.RemoveCustomerAsync(CustomerId);

            //Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task AddCustomer_WhenCorrectDataWasProvided_ThenReturnNewCustomerId_Test()
        {
            //Arrange
            const string lastName = "Kowalski";
            const string firstName = "Adam";
            ICustomerRepository stubCustomerRepository = new StubCustomerRepository();

            //Act
            int result = await stubCustomerRepository.AddCustomerAsync(firstName, lastName);

            //Assert
            result.Should().Be(4);
        }
    }
}