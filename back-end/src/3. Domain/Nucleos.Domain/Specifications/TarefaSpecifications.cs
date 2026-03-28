using System.Linq.Expressions;
using Nucleos.Domain.Entities;

namespace Nucleos.Domain.Specifications;

public class TarefasPendentesSpec : BaseSpecification<Tarefa>
{
    private readonly Guid _blocoId;
    public TarefasPendentesSpec(Guid blocoId) => _blocoId = blocoId;
    public override Expression<Func<Tarefa, bool>> Criteria => t => t.BlocoId == _blocoId && t.Status == "pendente" && t.DeletedAt == null;
}

public class TarefasVencendoSpec : BaseSpecification<Tarefa>
{
    private readonly DateTime _limite;
    public TarefasVencendoSpec(DateTime limite) => _limite = limite;
    public override Expression<Func<Tarefa, bool>> Criteria => t => t.DataVencimento <= _limite && t.Status != "concluida" && t.DeletedAt == null;
}
