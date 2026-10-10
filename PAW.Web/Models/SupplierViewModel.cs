using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class SupplierViewModel
{
    public PagedResult<SupplierDTO> Suppliers { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
