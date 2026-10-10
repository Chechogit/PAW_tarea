using PAW.DataAccess.MSSQL;
using PAW.Models;
using PAW.Repositories;

namespace PAW.DataAccess.Repositories;

public interface INotificationRepository : IRepositoryBase<Notification>
{
}

public class NotificationRepository(ProductDbContext context)
    : RepositoryBase<Notification>(context), INotificationRepository
{
}
