using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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
            throw new HttpRequestException(ReadErrorMessage(content, response), null, response.StatusCode);
        }
        return content;
    }

    /// <summary>
    /// When the API does BadRequest("text"), the text arrives as a JSON string: with quotes
    /// and with accents escaped (categor\u00eda). Deserializing it gives back the clean text.
    /// </summary>
    private static string? ReadErrorMessage(string content, HttpResponseMessage response)
    {
        if (string.IsNullOrWhiteSpace(content))
            return response.ReasonPhrase;

        if (content.StartsWith('"'))
        {
            try { return JsonSerializer.Deserialize<string>(content); }
            catch (JsonException) { }
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