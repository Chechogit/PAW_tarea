using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;
using System.Net;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<PagedResult<ProductDTO>> GetProductsAsync(int page);
    Task<ProductDTO?> GetProductAsync(int id);
    Task<ProductDTO> CreateProductAsync(ProductDTO product);
    Task UpdateProductAsync(ProductDTO product);
    Task DeleteProductAsync(int id);
}

public class ProductService : ServiceBase, IProductService
{
    private const string _path = "Product";
    private readonly IRestProvider _restProvider;

    public ProductService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<PagedResult<ProductDTO>> GetProductsAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<ProductDTO>>(response);
    }

    public async Task<ProductDTO?> GetProductAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
            return await JsonProvider.DeserializeAsync<ProductDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    public async Task<ProductDTO> CreateProductAsync(ProductDTO product)
    {
        var content = JsonProvider.Serialize(product);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<ProductDTO>(response);
    }

    public async Task UpdateProductAsync(ProductDTO product)
    {
        var content = JsonProvider.Serialize(product);
        await _restProvider.PutAsync(SetPathUrl(_path), product.ProductId.ToString(), content);
    }

    public async Task DeleteProductAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
    }
}