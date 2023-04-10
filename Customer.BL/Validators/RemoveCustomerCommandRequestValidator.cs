using Customer.Bl.Commands.RemoveCustomer;
using Customer.Bl.Validators.Base;
using FluentValidation;

namespace Customer.Bl.Validators
{
    public class RemoveCustomerCommandRequestValidator : BaseValidator<RemoveCustomerCommandRequest>
    {
        public RemoveCustomerCommandRequestValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThanOrEqualTo(1);
        }
    }
}
