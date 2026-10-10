using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class RoleViewModel
{
    public PagedResult<RoleDTO> Roles { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
