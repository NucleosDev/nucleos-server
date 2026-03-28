using FluentValidation;
using Nucleos.Application.Features.IA.Commands;

namespace Nucleos.Application.Features.IA.Validators;

public class GerarInsightValidator : AbstractValidator<GerarInsightCommand>
{
    public GerarInsightValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
