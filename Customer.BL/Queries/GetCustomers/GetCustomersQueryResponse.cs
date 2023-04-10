using Customer.Dal.Dto.Dtos;

namespace Customer.Bl.Queries.GetCustomers
{
    public class GetCustomersQueryResponse
    {
        public IEnumerable<CustomerDto> Customers { get; set; }
    }
}
