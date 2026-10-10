using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class UserRoleViewModel
{
    public PagedResult<UserRoleDTO> UserRoles { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
