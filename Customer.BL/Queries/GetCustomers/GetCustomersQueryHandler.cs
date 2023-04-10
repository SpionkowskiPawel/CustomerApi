using Customer.Dal.Dto.Dtos;
using Customer.Dal.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Customer.Bl.Queries.GetCustomers
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQueryRequest, GetCustomersQueryResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ILogger<GetCustomersQueryHandler> logger;

        public GetCustomersQueryHandler(ICustomerRepository customerRepository, ILogger<GetCustomersQueryHandler> logger)
        {
            this.customerRepository = customerRepository;
            this.logger = logger;
        }

        public async Task<GetCustomersQueryResponse> Handle(GetCustomersQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<CustomerDto> customers = await customerRepository.GetCustomersAsync();

                logger.LogInformation($"All consumer list has been provided.");
                return new GetCustomersQueryResponse() { Customers = customers };
            }
            catch (Exception ex)
            {
                logger.LogError($"The {nameof(GetCustomersQueryHandler)} operation failed. Stack trace: {ex.StackTrace}, Message: {ex.Message}.");
                throw new Exception("The server encountered an unexpected condition which prevented it from fulfilling the request.");
            }
        }
    }
}
