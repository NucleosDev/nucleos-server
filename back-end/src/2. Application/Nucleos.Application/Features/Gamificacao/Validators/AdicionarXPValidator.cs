using FluentValidation;
using Nucleos.Application.Features.Gamificacao.Commands;

namespace Nucleos.Application.Features.Gamificacao.Validators;

public class AdicionarXPValidator : AbstractValidator<AdicionarXPCommand>
{
    public AdicionarXPValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.XpAmount).GreaterThan(0);
        RuleFor(x => x.Source).NotEmpty();
    }
}
