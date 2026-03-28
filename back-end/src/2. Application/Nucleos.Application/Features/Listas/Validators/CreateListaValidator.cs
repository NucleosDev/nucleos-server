using FluentValidation;
using Nucleos.Application.Features.Listas.Commands;

namespace Nucleos.Application.Features.Listas.Validators;

public class CreateListaValidator : AbstractValidator<CreateListaCommand>
{
    public CreateListaValidator()
    {
        RuleFor(x => x.BlocoId).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
    }
}
