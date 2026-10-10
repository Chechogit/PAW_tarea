using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category>
{
}

public class CategoryRepository(ProductDbContext context) : RepositoryBase<Category>(context), ICategoryRepository
{
}
