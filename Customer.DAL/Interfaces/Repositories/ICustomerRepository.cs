using Customer.Dal.Dto.Dtos;

namespace Customer.Dal.Interfaces.Repositories
{
    /// <summary>
    /// Customer Repository
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Gets the all customers asynchronous.
        /// </summary>
        Task<IEnumerable<CustomerDto>> GetCustomersAsync();

        /// <summary>
        /// Adds the customer asynchronous.
        /// </summary>
        /// <param name="firstName">New customer FirstName.</param>
        /// <param name="lastName">New customer LastName.</param>
        Task<int> AddCustomerAsync(string firstName, string lastName);

        /// <summary>
        /// Removes the customer asynchronous.
        /// </summary>
        /// <param name="customerId">The removed customer identifier.</param>
        Task<bool> RemoveCustomerAsync(int customerId);
    }
}
