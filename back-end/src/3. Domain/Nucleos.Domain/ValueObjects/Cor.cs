namespace Nucleos.Domain.ValueObjects;
public record Cor(string Value)
{
    public static Cor Create(string hex) => new Cor(hex);
    public override string ToString() => Value;
}
