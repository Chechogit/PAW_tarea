using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class InventoryViewModel
{
    public PagedResult<InventoryDTO> Inventories { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
