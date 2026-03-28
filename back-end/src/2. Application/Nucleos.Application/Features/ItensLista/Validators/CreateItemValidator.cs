using FluentValidation;
using Nucleos.Application.Features.ItensLista.Commands;

namespace Nucleos.Application.Features.ItensLista.Validators;

public class CreateItemValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemValidator()
    {
        RuleFor(x => x.ListaId).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(500);
    }
}
