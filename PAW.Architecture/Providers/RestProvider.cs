using APW.Architecture.Helpers;

namespace APW.Architecture;

/// <summary>
/// Interface defining methods for RESTful operations.
/// </summary>
public interface IRestProvider
{
    Task<string> DeleteAsync(string endpoint, string id);
    Task<string> GetAsync(string endpoint, string? id);
    Task<string> PostAsync(string endpoint, string content);
    Task<string> PutAsync(string endpoint, string id, string content);
}

/// <summary>
/// Implementation of the IRestProvider interface, providing methods for RESTful operations.
/// </summary>
public class RestProvider : IRestProvider
{
    public async Task<string> GetAsync(string endpoint, string? id)
    {
        try
        {
            var response = await RestProviderHelpers.CreateHttpClient(endpoint)
                .GetAsync(RestProviderHelpers.BuildUrl(endpoint, id));        // CAMBIO
            return await RestProviderHelpers.GetResponse(response);
        }
        catch (Exception ex)
        {
            throw RestProviderHelpers.ThrowError(endpoint, ex);
        }
    }

    public async Task<string> PostAsync(string endpoint, string content)
    {
        try
        {
            var response = await RestProviderHelpers.CreateHttpClient(endpoint)
                .PostAsync(endpoint, RestProviderHelpers.CreateContent(content));
            return await RestProviderHelpers.GetResponse(response);
        }
        catch (Exception ex)
        {
            throw RestProviderHelpers.ThrowError(endpoint, ex);
        }
    }

    public async Task<string> PutAsync(string endpoint, string id, string content)
    {
        try
        {
            var response = await RestProviderHelpers.CreateHttpClient(endpoint)
                .PutAsync(RestProviderHelpers.BuildUrl(endpoint, id),          // CAMBIO
                          RestProviderHelpers.CreateContent(content));
            return await RestProviderHelpers.GetResponse(response);
        }
        catch (Exception ex)
        {
            throw RestProviderHelpers.ThrowError(endpoint, ex);
        }
    }

    public async Task<string> DeleteAsync(string endpoint, string id)
    {
        try
        {
            var response = await RestProviderHelpers.CreateHttpClient(endpoint)
                .DeleteAsync(RestProviderHelpers.BuildUrl(endpoint, id));      // CAMBIO
            return await RestProviderHelpers.GetResponse(response);
        }
        catch (Exception ex)
        {
            throw RestProviderHelpers.ThrowError(endpoint, ex);
        }
    }
}