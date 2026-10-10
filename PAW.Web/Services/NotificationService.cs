using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<PagedResult<NotificationDTO>> GetNotificationsAsync(int page);
    Task<NotificationDTO?> GetNotificationAsync(int id);
    Task<NotificationDTO> CreateNotificationAsync(NotificationDTO notification);
    Task UpdateNotificationAsync(NotificationDTO notification);
    Task DeleteNotificationAsync(int id);
}

public class NotificationService : ServiceBase, INotificationService
{
    private const string _path = "Notification";
    private readonly IRestProvider _restProvider;

    public NotificationService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /Notification?page=2&pageSize=25
    public async Task<PagedResult<NotificationDTO>> GetNotificationsAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<NotificationDTO>>(response);
    }

    // GET /Notification/5  → null si no existe
    public async Task<NotificationDTO?> GetNotificationAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<NotificationDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /Notification
    public async Task<NotificationDTO> CreateNotificationAsync(NotificationDTO notification)
    {
        var content = JsonProvider.Serialize(notification);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<NotificationDTO>(response);
    }

    // PUT /Notification/5
    public async Task UpdateNotificationAsync(NotificationDTO notification)
    {
        var content = JsonProvider.Serialize(notification);
        await _restProvider.PutAsync(SetPathUrl(_path), notification.Id.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /Notification/5
    public async Task DeleteNotificationAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
