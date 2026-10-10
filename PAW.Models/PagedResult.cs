using System.Text.Json.Serialization;

namespace PAW.Models;

/// <summary>
/// Configuración general de la paginación.
/// </summary>
public static class Paging
{
    /// <summary>Máximo de registros por página (lo pide la tarea).</summary>
    public const int MaxPageSize = 25;
}

/// <summary>
/// Lo que necesita la partial _Pager para dibujar los botones,
/// sin importar de qué entidad sea la lista.
/// </summary>
public interface IPagedResult
{
    int Page { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int TotalPages { get; }
}

/// <summary>
/// Una página de resultados: los registros de esa página + los datos para paginar.
/// </summary>
public class PagedResult<T> : IPagedResult
{
    [JsonPropertyName("items")]
    public IEnumerable<T> Items { get; set; } = [];

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Convierte los Items a otro tipo (por ejemplo, de entidad a DTO)
    /// conservando los datos de paginación.
    /// </summary>
    public PagedResult<TOut> Map<TOut>(Func<T, TOut> convert)
    {
        return new PagedResult<TOut>
        {
            Items = Items.Select(convert).ToList(),
            Page = Page,
            PageSize = PageSize,
            TotalCount = TotalCount
        };
    }
}