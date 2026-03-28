using FluentValidation;
using Nucleos.Application.Features.Tarefas.Commands;

namespace Nucleos.Application.Features.Tarefas.Validators;

public class CreateTarefaValidator : AbstractValidator<CreateTarefaCommand>
{
    public CreateTarefaValidator()
    {
        RuleFor(x => x.BlocoId).NotEmpty();
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(500);
    }
}
