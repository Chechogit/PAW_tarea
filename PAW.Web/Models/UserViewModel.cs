using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class UserViewModel
{
    public PagedResult<UserDTO> Users { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
