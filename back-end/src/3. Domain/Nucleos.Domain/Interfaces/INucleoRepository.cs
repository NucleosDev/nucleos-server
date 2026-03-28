using Nucleos.Domain.Entities;

namespace Nucleos.Domain.Interfaces;

public interface INucleoRepository : IRepository<Nucleo>
{
    Task<IEnumerable<Nucleo>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Nucleo?> GetWithBlocosAsync(Guid id, CancellationToken ct = default);
}
