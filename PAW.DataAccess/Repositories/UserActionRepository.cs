using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IUserActionRepository : IRepositoryBase<UserAction>
{
}

public class UserActionRepository(ProductDbContext context)
    : RepositoryBase<UserAction>(context), IUserActionRepository
{
}
