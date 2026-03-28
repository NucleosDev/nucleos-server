using Nucleos.Domain.Entities;

namespace Nucleos.Domain.Interfaces;

public interface IBlocoRepository : IRepository<Bloco>
{
    Task<IEnumerable<Bloco>> GetByNucleoIdAsync(Guid nucleoId, CancellationToken ct = default);
}
