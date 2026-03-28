namespace Nucleos.Domain.ValueObjects;
public record CPF(string Value)
{
    public static CPF Create(string value)
    {
        var clean = value.Replace(".", "").Replace("-", "");
        if (clean.Length != 11) throw new ArgumentException("CPF inválido.");
        return new CPF(clean);
    }
    public override string ToString() => Value;
}
