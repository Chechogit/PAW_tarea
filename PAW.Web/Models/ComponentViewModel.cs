using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class ComponentViewModel
{
    public PagedResult<ComponentDTO> Components { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
