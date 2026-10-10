using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class UserActionViewModel
{
    public PagedResult<UserActionDTO> UserActions { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
