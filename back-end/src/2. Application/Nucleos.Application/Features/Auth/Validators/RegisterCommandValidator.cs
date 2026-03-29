using FluentValidation;
using Nucleos.Application.Features.Auth.Commands;

namespace Nucleos.Application.Features.Auth.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório")
            .EmailAddress().WithMessage("E-mail inválido");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MinimumLength(3).WithMessage("Nome deve ter pelo menos 3 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("As senhas não conferem");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Must(BeValidCpf).WithMessage("CPF inválido");

        RuleFor(x => x.Phone)
            .Matches(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Telefone inválido. Use o formato (99) 99999-9999");
    }

    private bool BeValidCpf(string cpf)
    {
        var clean = new string(cpf.Where(char.IsDigit).ToArray());
        if (clean.Length != 11) return false;
        if (clean.Distinct().Count() == 1) return false;

        // Cálculo dos dígitos verificadores (igual ao seu método IsValidCpf)
        int sum = 0;
        for (int i = 0; i < 9; i++)
            sum += (clean[i] - '0') * (10 - i);
        int first = 11 - (sum % 11);
        if (first >= 10) first = 0;
        if (first != (clean[9] - '0')) return false;

        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += (clean[i] - '0') * (11 - i);
        int second = 11 - (sum % 11);
        if (second >= 10) second = 0;
        return second == (clean[10] - '0');
    }
}