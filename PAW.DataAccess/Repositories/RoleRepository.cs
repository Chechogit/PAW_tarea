using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IRoleRepository : IRepositoryBase<Role>
{
}

public class RoleRepository(ProductDbContext context)
    : RepositoryBase<Role>(context), IRoleRepository
{
}
