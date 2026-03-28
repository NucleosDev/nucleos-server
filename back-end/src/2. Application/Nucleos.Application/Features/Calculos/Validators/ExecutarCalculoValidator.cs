using FluentValidation;
using Nucleos.Application.Features.Calculos.Queries;

namespace Nucleos.Application.Features.Calculos.Validators;

public class ExecutarCalculoValidator : AbstractValidator<GetCalculoResultadoQuery>
{
    public ExecutarCalculoValidator()
    {
        RuleFor(x => x.BlocoId).NotEmpty();
    }
}
