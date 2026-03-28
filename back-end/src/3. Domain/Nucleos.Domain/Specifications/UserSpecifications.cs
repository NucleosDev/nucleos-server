using System.Linq.Expressions;
using Nucleos.Domain.Entities;

namespace Nucleos.Domain.Specifications;

public class UserByEmailSpec : BaseSpecification<User>
{
    private readonly string _email;
    public UserByEmailSpec(string email) => _email = email;
    public override Expression<Func<User, bool>> Criteria => u => u.Email == _email && u.DeletedAt == null;
}

public class UserActivosSpec : BaseSpecification<User>
{
    public override Expression<Func<User, bool>> Criteria => u => u.Active && u.DeletedAt == null;
}
