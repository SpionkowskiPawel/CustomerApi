using Customer.Bl.Commands.AddCustomer;
using Customer.Bl.Validators.Base;
using FluentValidation;

namespace Customer.Bl.Validators
{
    public class AddCustomerCommandRequestValidator : BaseValidator<AddCustomerCommandRequest>
    {
        public AddCustomerCommandRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
        }
    }
}
