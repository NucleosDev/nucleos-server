namespace Nucleos.Domain.ValueObjects;
public record Email(string Value)
{
    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
            throw new ArgumentException("Email inválido.", nameof(value));
        return new Email(value.ToLowerInvariant());
    }
    public override string ToString() => Value;
}
