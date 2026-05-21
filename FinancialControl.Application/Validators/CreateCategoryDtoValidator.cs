using FluentValidation;
using FinancialControl.Application.DTOs.CategoryDto;

namespace FinancialControl.Application.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
                
        }
    }
}
