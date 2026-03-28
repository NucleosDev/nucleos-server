namespace Nucleos.Infrastructure.Calculo.Operacoes;
public class MaxOperacao { public double Executar(IEnumerable<double> valores) { var l = valores.ToList(); return l.Any() ? l.Max() : 0; } }
