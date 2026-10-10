using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class ProductViewModel
{
    public PagedResult<ProductDTO> Products { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}