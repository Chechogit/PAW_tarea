using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IUserRoleRepository : IRepositoryBase<UserRole>
{
}

public class UserRoleRepository(ProductDbContext context)
    : RepositoryBase<UserRole>(context), IUserRoleRepository
{
}
