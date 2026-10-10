using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface IPawTaskRepository : IRepositoryBase<PawTask>
{
}

public class PawTaskRepository(ProductDbContext context)
    : RepositoryBase<PawTask>(context), IPawTaskRepository
{
}
