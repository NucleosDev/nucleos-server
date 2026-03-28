using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Interfaces;
using Nucleos.Infrastructure.Persistence.Context;

namespace Nucleos.Infrastructure.Persistence.Repositories;

public class BlocoRepository : Repository<Bloco>, IBlocoRepository
{
    public BlocoRepository(NucleosDbContext context) : base(context) { }

    public async Task<IEnumerable<Bloco>> GetByNucleoIdAsync(Guid nucleoId, CancellationToken ct = default)
        => await _context.Blocos.Where(b => b.NucleoId == nucleoId && b.DeletedAt == null).OrderBy(b => b.Posicao).ToListAsync(ct);
}
