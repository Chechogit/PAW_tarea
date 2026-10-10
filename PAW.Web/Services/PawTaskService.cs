using System.Globalization;
using System.Net;
using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IPawTaskService
{
    Task<PagedResult<PawTaskDTO>> GetPawTasksAsync(int page);
    Task<PawTaskDTO?> GetPawTaskAsync(int id);
    Task<PawTaskDTO> CreatePawTaskAsync(PawTaskDTO pawTask);
    Task UpdatePawTaskAsync(PawTaskDTO pawTask);
    Task DeletePawTaskAsync(int id);
}

public class PawTaskService : ServiceBase, IPawTaskService
{
    private const string _path = "PawTask";
    private readonly IRestProvider _restProvider;

    public PawTaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    // GET /PawTask?page=2&pageSize=25
    public async Task<PagedResult<PawTaskDTO>> GetPawTasksAsync(int page)
    {
        var url = SetPathUrl($"{_path}?page={page}&pageSize={Paging.MaxPageSize}");
        var response = await _restProvider.GetAsync(url, id: null);
        return await JsonProvider.DeserializeAsync<PagedResult<PawTaskDTO>>(response);
    }

    // GET /PawTask/5  → null si no existe
    public async Task<PawTaskDTO?> GetPawTaskAsync(int id)
    {
        try
        {
            var response = await _restProvider.GetAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
            return await JsonProvider.DeserializeAsync<PawTaskDTO>(response);
        }
        catch (ApplicationException ex) when (ex.InnerException is HttpRequestException { StatusCode: HttpStatusCode.NotFound })
        {
            return null;
        }
    }

    // POST /PawTask
    public async Task<PawTaskDTO> CreatePawTaskAsync(PawTaskDTO pawTask)
    {
        var content = JsonProvider.Serialize(pawTask);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return await JsonProvider.DeserializeAsync<PawTaskDTO>(response);
    }

    // PUT /PawTask/5
    public async Task UpdatePawTaskAsync(PawTaskDTO pawTask)
    {
        var content = JsonProvider.Serialize(pawTask);
        await _restProvider.PutAsync(SetPathUrl(_path), pawTask.Id.ToString(CultureInfo.InvariantCulture), content);
    }

    // DELETE /PawTask/5
    public async Task DeletePawTaskAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl(_path), id.ToString(CultureInfo.InvariantCulture));
    }
}
