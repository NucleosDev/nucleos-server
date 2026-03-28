namespace Nucleos.Infrastructure.Calculo.Operacoes;
public class MediaOperacao { public double Executar(IEnumerable<double> valores) { var l = valores.ToList(); return l.Any() ? l.Average() : 0; } }
