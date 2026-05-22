using FluentValidation;
using FinancialControl.Application.DTOs.TransactionDto;
namespace FinancialControl.Application.Validators
{
    public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionDto>
    {
        public UpdateTransactionValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
