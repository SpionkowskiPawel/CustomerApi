using MediatR;

namespace Customer.Bl.Commands.AddCustomer
{
    public class AddCustomerCommandRequest : IRequest<AddCustomerCommandResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
