using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IInventoryService
{
    Task<PagedResult<InventoryDTO>> GetInventoriesAsync(int page);
    Task<InventoryDTO?> GetInventoryAsync(int id);
    Task<InventoryDTO> CreateInventoryAsync(InventoryDTO inventory);
    Task UpdateInventoryAsync(InventoryDTO inventory);
    Task DeleteInventoryAsync(int id);
}

public class InventoryService : ServiceBase, IInventoryService
{
    private const string _path = "Inventory";
    private readonly IRestProvider _restProvider;

    public InventoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /Inventory?page=2&pageSize=25
    public async Task<PagedResult<InventoryDTO>> GetInventoriesAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<InventoryDTO>>(response);
    }

    // GET /Inventory/5  → null si no existe
    public async Task<InventoryDTO?> GetInventoryAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<InventoryDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /Inventory
    public async Task<InventoryDTO> CreateInventoryAsync(InventoryDTO inventory)
    {
        var content = JsonProvider.Serialize(inventory);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<InventoryDTO>(response);
    }

    // PUT /Inventory/5
    public async Task UpdateInventoryAsync(InventoryDTO inventory)
    {
        var content = JsonProvider.Serialize(inventory);
        await _restProvider.PutAsync(SetPathUrl(_path), inventory.InventoryId.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /Inventory/5
    public async Task DeleteInventoryAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
