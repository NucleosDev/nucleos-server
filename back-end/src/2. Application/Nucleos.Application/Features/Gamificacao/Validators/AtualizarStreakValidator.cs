using FluentValidation;
using Nucleos.Application.Features.Gamificacao.Commands;

namespace Nucleos.Application.Features.Gamificacao.Validators;

public class AtualizarStreakValidator : AbstractValidator<AtualizarStreakCommand>
{
    public AtualizarStreakValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.StreakType).NotEmpty();
    }
}
