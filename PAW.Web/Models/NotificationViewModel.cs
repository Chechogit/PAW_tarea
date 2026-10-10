using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class NotificationViewModel
{
    public PagedResult<NotificationDTO> Notifications { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
