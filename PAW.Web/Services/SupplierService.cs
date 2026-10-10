using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ISupplierService
{
    Task<PagedResult<SupplierDTO>> GetSuppliersAsync(int page);
    Task<SupplierDTO?> GetSupplierAsync(int id);
    Task<SupplierDTO> CreateSupplierAsync(SupplierDTO supplier);
    Task UpdateSupplierAsync(SupplierDTO supplier);
    Task DeleteSupplierAsync(int id);
}

public class SupplierService : ServiceBase, ISupplierService
{
    private const string _path = "Supplier";
    private readonly IRestProvider _restProvider;

    public SupplierService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /Supplier?page=2&pageSize=25
    public async Task<PagedResult<SupplierDTO>> GetSuppliersAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<SupplierDTO>>(response);
    }

    // GET /Supplier/5  → null si no existe
    public async Task<SupplierDTO?> GetSupplierAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<SupplierDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /Supplier
    public async Task<SupplierDTO> CreateSupplierAsync(SupplierDTO supplier)
    {
        var content = JsonProvider.Serialize(supplier);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<SupplierDTO>(response);
    }

    // PUT /Supplier/5
    public async Task UpdateSupplierAsync(SupplierDTO supplier)
    {
        var content = JsonProvider.Serialize(supplier);
        await _restProvider.PutAsync(SetPathUrl(_path), supplier.SupplierId.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /Supplier/5
    public async Task DeleteSupplierAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
