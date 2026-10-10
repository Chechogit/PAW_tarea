using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IRoleService
{
    Task<PagedResult<RoleDTO>> GetRolesAsync(int page);
    Task<RoleDTO?> GetRoleAsync(int id);
    Task<RoleDTO> CreateRoleAsync(RoleDTO role);
    Task UpdateRoleAsync(RoleDTO role);
    Task DeleteRoleAsync(int id);
}

public class RoleService : ServiceBase, IRoleService
{
    private const string _path = "Role";
    private readonly IRestProvider _restProvider;

    public RoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /Role?page=2&pageSize=25
    public async Task<PagedResult<RoleDTO>> GetRolesAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<RoleDTO>>(response);
    }

    // GET /Role/5  → null si no existe
    public async Task<RoleDTO?> GetRoleAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<RoleDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /Role
    public async Task<RoleDTO> CreateRoleAsync(RoleDTO role)
    {
        var content = JsonProvider.Serialize(role);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<RoleDTO>(response);
    }

    // PUT /Role/5
    public async Task UpdateRoleAsync(RoleDTO role)
    {
        var content = JsonProvider.Serialize(role);
        await _restProvider.PutAsync(SetPathUrl(_path), role.RoleId.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /Role/5
    public async Task DeleteRoleAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
