using FluentValidation;
using Nucleos.Application.Features.Admin.Commands;

namespace Nucleos.Application.Features.Admin.Validators;

public class ManageSubscriptionValidator : AbstractValidator<ManageSubscriptionCommand>
{
    public ManageSubscriptionValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PlanId).NotEmpty();
        RuleFor(x => x.DurationDays).GreaterThan(0);
    }
}
