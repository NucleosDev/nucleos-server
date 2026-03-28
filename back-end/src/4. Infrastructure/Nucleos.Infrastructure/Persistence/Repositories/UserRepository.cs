using Microsoft.EntityFrameworkCore;
using Nucleos.Domain.Entities;
using Nucleos.Domain.Interfaces;
using Nucleos.Infrastructure.Persistence.Context;

namespace Nucleos.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(NucleosDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _context.Users.Include(u => u.Profile).Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => await _context.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<bool> CpfExistsAsync(string cpf, CancellationToken ct = default)
        => await _context.Users.AnyAsync(u => u.Cpf == cpf, ct);
}
