using FluentValidation;
using Nucleos.Application.Features.Blocos.Commands;

namespace Nucleos.Application.Features.Blocos.Validators;

public class CreateBlocoValidator : AbstractValidator<CreateBlocoCommand>
{
    public CreateBlocoValidator()
    {
        RuleFor(x => x.NucleoId).NotEmpty();
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Tipo).NotEmpty();
    }
}
