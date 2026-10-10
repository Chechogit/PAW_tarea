using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ICategoryService
{
    Task<PagedResult<CategoryDTO>> GetCategoriesAsync(int page);
    Task<CategoryDTO?> GetCategoryAsync(int id);
    Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);
    Task UpdateCategoryAsync(CategoryDTO category);
    Task DeleteCategoryAsync(int id);
}

public class CategoryService : ServiceBase, ICategoryService
{
    private const string _path = "Category";
    private readonly IRestProvider _restProvider;

    public CategoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /Category?page=2&pageSize=25
    public async Task<PagedResult<CategoryDTO>> GetCategoriesAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<CategoryDTO>>(response);
    }

    // GET /Category/5  → null si no existe
    public async Task<CategoryDTO?> GetCategoryAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString());
            return await JsonProvider.DeserializeAsync<CategoryDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /Category
    public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category)
    {
        var content = JsonProvider.Serialize(category);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<CategoryDTO>(response);
    }

    // PUT /Category/5
    public async Task UpdateCategoryAsync(CategoryDTO category)
    {
        var content = JsonProvider.Serialize(category);
        await _restProvider.PutAsync(SetPathUrl(_path), category.CategoryId.ToString(), content);
    }

    // DELETE /Category/5
    public async Task DeleteCategoryAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString());
    }
}
