using PAW.Models;
using PAW.Models.DTO;
using PAW.Web.Services;

namespace PAW.Web.Models;

public class PawTaskViewModel
{
    public PagedResult<PawTaskDTO> PawTasks { get; set; } = new();

    public TipoCambioResponse TipoCambio { get; set; }
}
