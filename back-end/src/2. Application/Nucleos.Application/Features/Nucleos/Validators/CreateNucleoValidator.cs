using FluentValidation;
using Nucleos.Application.Features.Nucleos.Commands;

namespace Nucleos.Application.Features.Nucleos.Validators;

public class CreateNucleoValidator : AbstractValidator<CreateNucleoCommand>
{
    public CreateNucleoValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
    }
}
