using Customer.Dal.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customer.Bl.Commands.RemoveCustomer
{
    public class RemoveCustomerCommandHandler : IRequestHandler<RemoveCustomerCommandRequest, RemoveCustomerCommandResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<RemoveCustomerCommandHandler> logger;

        public RemoveCustomerCommandHandler(ICustomerRepository customerRepository, ILogger<RemoveCustomerCommandHandler> logger)
        {
            this.customerRepository = customerRepository;
            this.logger = logger;
        }

        public async Task<RemoveCustomerCommandResponse> Handle(RemoveCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                bool isSuccess = await customerRepository.RemoveCustomerAsync(request.CustomerId);

                logger.LogInformation($"Consumer with consumerId = {request.CustomerId} has been { (isSuccess ? "removed" : "not removed")}.");
                return new RemoveCustomerCommandResponse() { Success = isSuccess };
            }
            catch (Exception ex)
            {
                logger.LogError($"The {nameof(RemoveCustomerCommandHandler)} operation failed. Stack trace: {ex.StackTrace}, Message: {ex.Message}.");
                throw new Exception("The server encountered an unexpected condition which prevented it from fulfilling the request.");
            }
        }
    }
}
