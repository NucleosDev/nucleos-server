namespace Nucleos.Domain.ValueObjects;
public record Telefone(string Value)
{
    public static Telefone Create(string value) => new Telefone(value);
    public override string ToString() => Value;
}
