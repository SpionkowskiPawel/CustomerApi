using Customer.Bl.Queries.GetCustomers;
using Customer.Dal.Dto.Dtos;
using Customer.Dal.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;

namespace Consumer.UnitTest.Handlers.Queries
{
    public class GetCustomersQueryHandlerTests
    {
        [Test]
        public async Task GetCustomersQuery_WhenCustomersExists_ThenReturnSuccess()
        {
            //Arrange
            IEnumerable<CustomerDto> exceptedCustomers = new List<CustomerDto>() {
                new CustomerDto() { Id = 1, LastName = "Kowalski", FirstName = "Adam"},
                new CustomerDto() { Id = 2, LastName = "Nowak", FirstName = "Karolina"}
            };
            Mock<ICustomerRepository> mockCustomerRepository = new();

            mockCustomerRepository
                .Setup(s => s.GetCustomersAsync())
                .Returns(Task.FromResult<IEnumerable<CustomerDto>>(exceptedCustomers));

            Mock<ILogger<GetCustomersQueryHandler>> mockLogger = new();
            GetCustomersQueryHandler queryHandler = new GetCustomersQueryHandler(mockCustomerRepository.Object, mockLogger.Object);

            //Act
            GetCustomersQueryResponse getCustomersQueryResponse = await queryHandler.Handle(new GetCustomersQueryRequest(), new CancellationToken());

            //Assert
            getCustomersQueryResponse.Should().NotBeNull();
            getCustomersQueryResponse.Customers.Should().NotBeNull();
            getCustomersQueryResponse.Customers.Should().HaveCount(exceptedCustomers.Count());
            getCustomersQueryResponse.Customers.First().Id.Should().Be(exceptedCustomers.First().Id);
            getCustomersQueryResponse.Customers.First().LastName.Should().Be(exceptedCustomers.First().LastName);
            getCustomersQueryResponse.Customers.First().FirstName.Should().Be(exceptedCustomers.First().FirstName);

            mockCustomerRepository.Verify(v => v.GetCustomersAsync(), Times.Once());
            mockLogger.Verify(
                x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => true),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
            mockLogger.Verify(
                x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => true),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
        }


        [Test]
        public async Task GetCustomersQuery_WhenCustomerRepositoryThrow_ThenThrowException()
        {
            //Arrange
            const string exceptedException = "The server encountered an unexpected condition which prevented it from fulfilling the request.";
            IEnumerable<CustomerDto> exceptedCustomers = new List<CustomerDto>() {
                new CustomerDto() { Id = 1, LastName = "Kowalski", FirstName = "Adam"},
                new CustomerDto() { Id = 2, LastName = "Nowak", FirstName = "Karolina"}
            };
            Mock<ICustomerRepository> mockCustomerRepository = new();

            mockCustomerRepository
                .Setup(s => s.GetCustomersAsync())
                .ThrowsAsync(new Exception());

            Mock<ILogger<GetCustomersQueryHandler>> mockLogger = new();
            GetCustomersQueryHandler queryHandler = new GetCustomersQueryHandler(mockCustomerRepository.Object, mockLogger.Object);

            //Act
            Func<Task<GetCustomersQueryResponse>> act = () => queryHandler.Handle(new GetCustomersQueryRequest(), new CancellationToken());

            //Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage(exceptedException);

            mockCustomerRepository.Verify(v => v.GetCustomersAsync(), Times.Once());
            mockLogger.Verify(
                x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => true),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);

        }
    }
}
