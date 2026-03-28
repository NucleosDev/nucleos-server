using FluentValidation;
using Nucleos.Application.Features.Admin.Commands;

namespace Nucleos.Application.Features.Admin.Validators;

public class CreatePlanValidator : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}
