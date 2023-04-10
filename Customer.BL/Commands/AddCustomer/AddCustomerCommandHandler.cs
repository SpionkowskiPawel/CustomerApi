using Customer.Bl.Queries.GetCustomers;
using Customer.Dal.Dto.Dtos;
using Customer.Dal.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customer.Bl.Commands.AddCustomer
{
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommandRequest, AddCustomerCommandResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<GetCustomersQueryHandler> logger;

        public AddCustomerCommandHandler(ICustomerRepository customerRepository, ILogger<GetCustomersQueryHandler> logger)
        {
            this.customerRepository = customerRepository;
            this.logger = logger;
        }

        public async Task<AddCustomerCommandResponse> Handle(AddCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<CustomerDto> users = await customerRepository.GetCustomersAsync();

                if(users.Any(x => x.FirstName == request.FirstName && x.LastName == request.LastName))
                {
                    logger.LogWarning($"An attempt was made to add a user that exists: FirstName = { request.FirstName}, LastName = {request.LastName}.");
                    return new AddCustomerCommandResponse() {Success = false };
                }

                int newCustomerId = await customerRepository.AddCustomerAsync(request.FirstName, request.LastName);

                logger.LogInformation($"New Consumer has been added with consumerId = {newCustomerId}.");
                return new AddCustomerCommandResponse() { CustomerId = newCustomerId, Success = true };
            }
            catch (Exception ex)
            {
                logger.LogError($"The {nameof(AddCustomerCommandHandler)} operation failed. Stack trace: {ex.StackTrace}, Message: {ex.Message}.");
                throw new Exception("The server encountered an unexpected condition which prevented it from fulfilling the request.");
            }
        }
    }
}
