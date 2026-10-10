using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IUserRoleService
{
    Task<PagedResult<UserRoleDTO>> GetUserRolesAsync(int page);
    Task<UserRoleDTO?> GetUserRoleAsync(decimal id);
    Task<UserRoleDTO> CreateUserRoleAsync(UserRoleDTO userRole);
    Task UpdateUserRoleAsync(UserRoleDTO userRole);
    Task DeleteUserRoleAsync(decimal id);
}

public class UserRoleService : ServiceBase, IUserRoleService
{
    private const string _path = "UserRole";
    private readonly IRestProvider _restProvider;

    public UserRoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /UserRole?page=2&pageSize=25
    public async Task<PagedResult<UserRoleDTO>> GetUserRolesAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<UserRoleDTO>>(response);
    }

    // GET /UserRole/5  → null si no existe
    public async Task<UserRoleDTO?> GetUserRoleAsync(decimal id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<UserRoleDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /UserRole
    public async Task<UserRoleDTO> CreateUserRoleAsync(UserRoleDTO userRole)
    {
        var content = JsonProvider.Serialize(userRole);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<UserRoleDTO>(response);
    }

    // PUT /UserRole/5
    public async Task UpdateUserRoleAsync(UserRoleDTO userRole)
    {
        var content = JsonProvider.Serialize(userRole);
        await _restProvider.PutAsync(SetPathUrl(_path), userRole.Id.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /UserRole/5
    public async Task DeleteUserRoleAsync(decimal id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
