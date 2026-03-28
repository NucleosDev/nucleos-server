using FluentValidation;
using Nucleos.Application.Features.IA.Commands;

namespace Nucleos.Application.Features.IA.Validators;

public class EnviarMensagemValidator : AbstractValidator<EnviarMensagemCommand>
{
    public EnviarMensagemValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Mensagem).NotEmpty().MaximumLength(4000);
    }
}
