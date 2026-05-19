using FinancialControl.Application.DTOs;
using FluentValidation;

namespace FinancialControl.Application.Validators
{
    public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
    {
        public CreateTransactionDtoValidator()
        {
            RuleFor( x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);
            RuleFor(x => x.Type)
                .InclusiveBetween(1,2);
        }
    }
}
