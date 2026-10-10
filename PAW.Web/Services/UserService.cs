using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserService
{
    Task<PagedResult<UserDTO>> GetUsersAsync(int page);
    Task<UserDTO?> GetUserAsync(int id);
    Task<UserDTO> CreateUserAsync(UserDTO user);
    Task UpdateUserAsync(UserDTO user);
    Task DeleteUserAsync(int id);
}

public class UserService : ServiceBase, IUserService
{
    private const string _path = "User";
    private readonly IRestProvider _restProvider;

    public UserService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /User?page=2&pageSize=25
    public async Task<PagedResult<UserDTO>> GetUsersAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<UserDTO>>(response);
    }

    // GET /User/5  → null si no existe
    public async Task<UserDTO?> GetUserAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<UserDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /User
    public async Task<UserDTO> CreateUserAsync(UserDTO user)
    {
        var content = JsonProvider.Serialize(user);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<UserDTO>(response);
    }

    // PUT /User/5
    public async Task UpdateUserAsync(UserDTO user)
    {
        var content = JsonProvider.Serialize(user);
        await _restProvider.PutAsync(SetPathUrl(_path), user.UserId.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /User/5
    public async Task DeleteUserAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
