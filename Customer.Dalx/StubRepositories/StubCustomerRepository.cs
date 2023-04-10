using Customer.Dal.Dto.Dtos;
using Customer.Dal.Interfaces.Repositories;

namespace Customer.Dal.StubRepositories
{
    public class StubCustomerRepository : ICustomerRepository
    {
        public Task<int> AddCustomerAsync(string firstName, string lastName)
        {
            return Task.FromResult(4);
        }

        public Task<IEnumerable<CustomerDto>> GetCustomersAsync()
        {
            IEnumerable<CustomerDto> result = new List<CustomerDto>()
            {
                new CustomerDto() { Id = 1, FirstName = "Max", LastName = "Tionge"},
                new CustomerDto() { Id = 2, FirstName = "Donnchad", LastName = "Hristofor"},
                new CustomerDto() { Id = 3, FirstName = "Jockel", LastName = "Yamila"}
            };

            return Task.FromResult(result);
        }

        public Task<bool> RemoveCustomerAsync(int customerId)
        {
            return Task.FromResult(true);
        }
    }
}
