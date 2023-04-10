using MediatR;

namespace Customer.Bl.Commands.RemoveCustomer
{
    public class RemoveCustomerCommandRequest : IRequest<RemoveCustomerCommandResponse>
    {
        public int CustomerId { get; set; }
    }
}
