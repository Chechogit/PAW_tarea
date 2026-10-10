using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IComponentService
{
    Task<PagedResult<ComponentDTO>> GetComponentsAsync(int page);
    Task<ComponentDTO?> GetComponentAsync(decimal id);
    Task<ComponentDTO> CreateComponentAsync(ComponentDTO component);
    Task UpdateComponentAsync(ComponentDTO component);
    Task DeleteComponentAsync(decimal id);
}

public class ComponentService : ServiceBase, IComponentService
{
    private const string _path = "Component";
    private readonly IRestProvider _restProvider;

    public ComponentService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /Component?page=2&pageSize=25
    public async Task<PagedResult<ComponentDTO>> GetComponentsAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<ComponentDTO>>(response);
    }

    // GET /Component/5  → null si no existe
    public async Task<ComponentDTO?> GetComponentAsync(decimal id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<ComponentDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /Component
    public async Task<ComponentDTO> CreateComponentAsync(ComponentDTO component)
    {
        var content = JsonProvider.Serialize(component);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<ComponentDTO>(response);
    }

    // PUT /Component/5
    public async Task UpdateComponentAsync(ComponentDTO component)
    {
        var content = JsonProvider.Serialize(component);
        await _restProvider.PutAsync(SetPathUrl(_path), component.Id.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /Component/5
    public async Task DeleteComponentAsync(decimal id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
