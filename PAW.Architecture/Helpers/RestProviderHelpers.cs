using System.Net.Http.Headers;
using System.Text;

namespace APW.Architecture.Helpers;

/// <summary>
/// Provides helper methods for creating and managing HTTP requests.
/// </summary>
internal static class RestProviderHelpers
{
    /// <summary>
    /// Creates an <see cref="HttpClient"/> with the specified base address.
    /// </summary>
    internal static HttpClient CreateHttpClient(string endpoint)
    {
        var client = new HttpClient { BaseAddress = new Uri(endpoint) };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    /// <summary>
    /// Creates a <see cref="StringContent"/> object for the specified content.
    /// </summary>
    internal static StringContent CreateContent(string content) => new(content, Encoding.UTF8, "application/json");

    /// <summary>
    /// Reads the response. If the API answered with an error (400, 404, 500...),
    /// throws an HttpRequestException carrying the API message and the status code.
    /// </summary>
    internal static async Task<string> GetResponse(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            var message = string.IsNullOrWhiteSpace(content) ? response.ReasonPhrase : content.Trim('"');
            throw new HttpRequestException(message, null, response.StatusCode);
        }
        return content;
    }

    /// <summary>
    /// Builds the full URL: endpoint + "/" + id. If there is no id, returns the endpoint as is.
    /// </summary>
    internal static string BuildUrl(string endpoint, string? id)
        => string.IsNullOrWhiteSpace(id) ? endpoint : $"{endpoint.TrimEnd('/')}/{id}";

    /// <summary>
    /// Creates an <see cref="ApplicationException"/> with details about an error occurring during data retrieval.
    /// </summary>
    internal static Exception ThrowError(string endpoint, Exception ex) => new ApplicationException($"Error getting data from {endpoint}", ex);
}