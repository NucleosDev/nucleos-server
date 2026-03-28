using FluentValidation;
using Nucleos.Application.Features.Habitos.Commands;

namespace Nucleos.Application.Features.Habitos.Validators;

public class CreateHabitoValidator : AbstractValidator<CreateHabitoCommand>
{
    public CreateHabitoValidator()
    {
        RuleFor(x => x.BlocoId).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Frequencia).NotEmpty();
    }
}
