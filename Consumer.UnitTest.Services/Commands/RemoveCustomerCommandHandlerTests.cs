using Customer.Dal.Dto.Dtos;
using Customer.Dal.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using FluentAssertions;
using NUnit.Framework.Internal;
using Customer.Bl.Commands.RemoveCustomer;

namespace Consumer.UnitTest.Handlers.Commands
{
    public class RemoveCustomerCommandHandlerTests
    {
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public async Task RemoveCustomerCommand_WhenCustomerWasRemovedAndNotRemoved_ThenReturnFailure(bool expectedResult)
        {
            //Arrange
            const int CustomerIdToRemove = 1;
            IEnumerable<CustomerDto> exceptedCustomers = new List<CustomerDto>() {
                new CustomerDto() { Id = 1, LastName = "Kowalski", FirstName = "Adam"},
                new CustomerDto() { Id = 2, LastName = "Nowak", FirstName = "Karolina"}
            };
            RemoveCustomerCommandRequest removeCustomerCommandRequest = new() { CustomerId = 1 };
            Mock<ICustomerRepository> mockCustomerRepository = new();

            mockCustomerRepository
                .Setup(s => s.RemoveCustomerAsync(CustomerIdToRemove))
                .Returns(Task.FromResult(expectedResult));

            Mock<ILogger<RemoveCustomerCommandHandler>> mockLogger = new();
            RemoveCustomerCommandHandler queryHandler = new RemoveCustomerCommandHandler(mockCustomerRepository.Object, mockLogger.Object);

            //Act
            RemoveCustomerCommandResponse removeCustomerCommandResponse = await queryHandler.Handle(removeCustomerCommandRequest, new CancellationToken());

            //Assert
            removeCustomerCommandResponse.Should().NotBeNull();
            removeCustomerCommandResponse.Success.Should().Be(expectedResult);

            mockCustomerRepository.Verify(v => v.RemoveCustomerAsync(It.IsAny<int>()), Times.Once());
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
        public async Task RemoveCustomerCommand_WhenCustomerWasNotRemoved_ThenThrowException()
        {
            //Arrange
            const string exceptedException = "The server encountered an unexpected condition which prevented it from fulfilling the request.";
            const int CustomerIdToRemove = 1;
            IEnumerable<CustomerDto> exceptedCustomers = new List<CustomerDto>() {
                new CustomerDto() { Id = 1, LastName = "Kowalski", FirstName = "Adam"},
                new CustomerDto() { Id = 2, LastName = "Nowak", FirstName = "Karolina"}
            };
            RemoveCustomerCommandRequest removeCustomerCommandRequest = new() { CustomerId = 1 };
            Mock<ICustomerRepository> mockCustomerRepository = new();

            mockCustomerRepository
                .Setup(s => s.RemoveCustomerAsync(CustomerIdToRemove))
                .ThrowsAsync(new Exception());

            Mock<ILogger<RemoveCustomerCommandHandler>> mockLogger = new();
            RemoveCustomerCommandHandler queryHandler = new RemoveCustomerCommandHandler(mockCustomerRepository.Object, mockLogger.Object);

            //Act
            Func<Task<RemoveCustomerCommandResponse>> act = () => queryHandler.Handle(removeCustomerCommandRequest, new CancellationToken());

            //Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage(exceptedException);

            mockCustomerRepository.Verify(v => v.RemoveCustomerAsync(It.IsAny<int>()), Times.Once());
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
