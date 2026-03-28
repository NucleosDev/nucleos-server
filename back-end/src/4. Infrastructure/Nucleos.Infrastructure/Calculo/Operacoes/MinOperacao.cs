namespace Nucleos.Infrastructure.Calculo.Operacoes;
public class MinOperacao { public double Executar(IEnumerable<double> valores) { var l = valores.ToList(); return l.Any() ? l.Min() : 0; } }
