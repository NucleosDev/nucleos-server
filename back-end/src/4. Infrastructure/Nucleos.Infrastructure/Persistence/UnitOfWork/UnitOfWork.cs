using System.Threading;
using System.Threading.Tasks;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Infrastructure.Persistence.Context;

namespace Nucleos.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly NucleosDbContext _context;

    public UnitOfWork(NucleosDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}