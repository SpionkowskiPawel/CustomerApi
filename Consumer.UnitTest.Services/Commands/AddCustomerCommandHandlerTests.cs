using Customer.Bl.Queries.GetCustomers;
using Customer.Dal.Dto.Dtos;
using Customer.Dal.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;
using Customer.Bl.Commands.AddCustomer;

namespace Consumer.UnitTest.Handlers.Commands
{
    public class AddCustomerCommandHandlerTests
    {
        [Test]
        public async Task AddCustomerCommand_WhenValidationFailed_ThenReturnFailure()
        {
            //Arrange
            IEnumerable<CustomerDto> exceptedCustomers = new List<CustomerDto>() {
                new CustomerDto() { Id = 1, LastName = "Kowalski", FirstName = "Adam"},
                new CustomerDto() { Id = 2, LastName = "Nowak", FirstName = "Karolina"}
            };
            AddCustomerCommandRequest addCustomerCommandRequest = new() { FirstName = "Karolina", LastName = "Nowak" };
            Mock<ICustomerRepository> mockCustomerRepository = new();

            mockCustomerRepository
                .Setup(s => s.GetCustomersAsync())
                .Returns(Task.FromResult(exceptedCustomers));

            Mock<ILogger<GetCustomersQueryHandler>> mockLogger = new();
            AddCustomerCommandHandler queryHandler = new AddCustomerCommandHandler(mockCustomerRepository.Object, mockLogger.Object);

            //Act
            AddCustomerCommandResponse addCustomerCommandResponse = await queryHandler.Handle(addCustomerCommandRequest, new CancellationToken());

            //Assert
            addCustomerCommandResponse.Should().NotBeNull();
            addCustomerCommandResponse.Success.Should().BeFalse();

            mockCustomerRepository.Verify(v => v.GetCustomersAsync(), Times.Once());
            mockCustomerRepository.Verify(v => v.AddCustomerAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            mockLogger.Verify(
                x => x.Log(
                        LogLevel.Warning,
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
        public async Task AddCustomerCommand_WhenCustomerWasAdded_ThenReturnNewCustomerId()
        {
            //Arrange
            IEnumerable<CustomerDto> exceptedCustomers = new List<CustomerDto>() {
                new CustomerDto() { Id = 1, LastName = "Kowalski", FirstName = "Adam"},
                new CustomerDto() { Id = 2, LastName = "Nowak", FirstName = "Karolina"}
            };
            AddCustomerCommandRequest addCustomerCommandRequest = new() { FirstName = "Sebastian", LastName = "Lewandowski" };
            Mock<ICustomerRepository> mockCustomerRepository = new();

            mockCustomerRepository
                .Setup(s => s.GetCustomersAsync())
                .Returns(Task.FromResult(exceptedCustomers));

            Mock<ILogger<GetCustomersQueryHandler>> mockLogger = new();
            AddCustomerCommandHandler queryHandler = new AddCustomerCommandHandler(mockCustomerRepository.Object, mockLogger.Object);

            //Act
            AddCustomerCommandResponse addCustomerCommandResponse = await queryHandler.Handle(addCustomerCommandRequest, new CancellationToken());

            //Assert
            addCustomerCommandResponse.Should().NotBeNull();
            addCustomerCommandResponse.Success.Should().BeTrue();

            mockCustomerRepository.Verify(v => v.GetCustomersAsync(), Times.Once());
            mockCustomerRepository.Verify(v => v.AddCustomerAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
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
        public async Task AddCustomerCommand_WhenCustomerRepositoryThrow_ThenThrowException()
        {
            //Arrange
            const string exceptedException = "The server encountered an unexpected condition which prevented it from fulfilling the request.";
            IEnumerable<CustomerDto> exceptedCustomers = new List<CustomerDto>() {
                new CustomerDto() { Id = 1, LastName = "Kowalski", FirstName = "Adam"},
                new CustomerDto() { Id = 2, LastName = "Nowak", FirstName = "Karolina"}
            };
            AddCustomerCommandRequest addCustomerCommandRequest = new() { FirstName = "Sebastian", LastName = "Lewandowski" };
            Mock<ICustomerRepository> mockCustomerRepository = new();

            mockCustomerRepository
                .Setup(s => s.GetCustomersAsync())
                .ThrowsAsync(new Exception());

            Mock<ILogger<GetCustomersQueryHandler>> mockLogger = new();
            AddCustomerCommandHandler queryHandler = new AddCustomerCommandHandler(mockCustomerRepository.Object, mockLogger.Object);

            //Act
            Func<Task<AddCustomerCommandResponse>> act = () => queryHandler.Handle(addCustomerCommandRequest, new CancellationToken());

            //Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage(exceptedException);

            mockCustomerRepository.Verify(v => v.GetCustomersAsync(), Times.Once());
            mockCustomerRepository.Verify(v => v.AddCustomerAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            mockLogger.Verify(
                x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => true),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Never);
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
