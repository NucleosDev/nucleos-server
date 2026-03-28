using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nucleos.Application.Common.Interfaces;
using Nucleos.Infrastructure.Persistence.Context;

namespace Nucleos.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T>, Domain.Interfaces.IRepository<T> where T : class
{
    protected readonly NucleosDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(NucleosDbContext context) { _context = context; _dbSet = context.Set<T>(); }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) => await _dbSet.FindAsync(new object[] { id }, ct);
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default) => await _dbSet.ToListAsync(ct);
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) => await _dbSet.Where(predicate).ToListAsync(ct);
    public async Task AddAsync(T entity, CancellationToken ct = default) => await _dbSet.AddAsync(entity, ct);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Remove(T entity) => _dbSet.Remove(entity);
}
