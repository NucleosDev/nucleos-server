using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Interfaces;
using Nucleos.Infrastructure.Persistence.Context;

namespace Nucleos.Infrastructure.Persistence.Repositories;

public class NucleoRepository : Repository<Nucleo>, INucleoRepository
{
    public NucleoRepository(NucleosDbContext context) : base(context) { }

    public async Task<IEnumerable<Nucleo>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.Nucleos.Where(n => n.UserId == userId && n.DeletedAt == null).OrderByDescending(n => n.CreatedAt).ToListAsync(ct);

    public async Task<Nucleo?> GetWithBlocosAsync(Guid id, CancellationToken ct = default)
        => await _context.Nucleos.Include(n => n.Blocos.Where(b => b.DeletedAt == null)).FirstOrDefaultAsync(n => n.Id == id && n.DeletedAt == null, ct);
}
