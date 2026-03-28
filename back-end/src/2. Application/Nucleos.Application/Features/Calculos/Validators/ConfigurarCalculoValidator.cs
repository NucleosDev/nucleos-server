using FluentValidation;
using Nucleos.Application.Features.Calculos.Commands;

namespace Nucleos.Application.Features.Calculos.Validators;

public class ConfigurarCalculoValidator : AbstractValidator<ConfigurarCalculoCommand>
{
    public ConfigurarCalculoValidator()
    {
        RuleFor(x => x.BlocoId).NotEmpty();
        RuleFor(x => x.TipoOperacao).NotEmpty();
        RuleFor(x => x.Campo).NotEmpty();
    }
}
