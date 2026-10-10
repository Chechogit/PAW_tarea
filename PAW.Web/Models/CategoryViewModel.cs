using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class CategoryViewModel
{
    public PagedResult<CategoryDTO> Categories { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
