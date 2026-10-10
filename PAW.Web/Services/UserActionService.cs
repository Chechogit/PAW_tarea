using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserActionService
{
    Task<PagedResult<UserActionDTO>> GetUserActionsAsync(int page);
    Task<UserActionDTO?> GetUserActionAsync(decimal id);
    Task<UserActionDTO> CreateUserActionAsync(UserActionDTO userAction);
    Task UpdateUserActionAsync(UserActionDTO userAction);
    Task DeleteUserActionAsync(decimal id);
}

public class UserActionService : ServiceBase, IUserActionService
{
    private const string _path = "UserAction";
    private readonly IRestProvider _restProvider;

    public UserActionService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /UserAction?page=2&pageSize=25
    public async Task<PagedResult<UserActionDTO>> GetUserActionsAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<UserActionDTO>>(response);
    }

    // GET /UserAction/5  → null si no existe
    public async Task<UserActionDTO?> GetUserActionAsync(decimal id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<UserActionDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /UserAction
    public async Task<UserActionDTO> CreateUserActionAsync(UserActionDTO userAction)
    {
        var content = JsonProvider.Serialize(userAction);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<UserActionDTO>(response);
    }

    // PUT /UserAction/5
    public async Task UpdateUserActionAsync(UserActionDTO userAction)
    {
        var content = JsonProvider.Serialize(userAction);
        await _restProvider.PutAsync(SetPathUrl(_path), userAction.Id.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /UserAction/5
    public async Task DeleteUserActionAsync(decimal id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
