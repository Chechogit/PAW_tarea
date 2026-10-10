using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController(ILogger<UserRoleController> logger, IUserRoleRepository userRoleRepository) : ControllerBase
    {
        // GET /UserRole?page=1&pageSize=25
        [HttpGet(Name = "GetUserRoles")]
        public async Task<PagedResult<UserRoleDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await userRoleRepository.ReadPagedAsync(page, pageSize);
            return result.Map(UserRoleDTO.ConvertFrom);
        }

        // GET /UserRole/5
        [HttpGet("{id:decimal}", Name = "GetUserRoleById")]
        public async Task<ActionResult<UserRoleDTO>> GetById(decimal id)
        {
            var userRole = await userRoleRepository.FindAsync(id);
            if (userRole is null)
                return NotFound();

            return UserRoleDTO.ConvertFrom(userRole);
        }

        // POST /UserRole
        [HttpPost]
        public async Task<ActionResult<UserRoleDTO>> Create([FromBody] UserRoleDTO userRoleDTO)
        {
            var userRole = UserRoleDTO.ConvertTo(userRoleDTO);
            userRole.Id = 0m;                          // el Id lo genera SQL Server (IDENTITY)

            try
            {
                await userRoleRepository.CreateAsync(userRole);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear la asignación de rol");
                return BadRequest("No se pudo crear la asignación de rol.");
            }

            return CreatedAtRoute("GetUserRoleById", new { id = userRole.Id }, UserRoleDTO.ConvertFrom(userRole));
        }

        // PUT /UserRole/5
        [HttpPut("{id:decimal}")]
        public async Task<IActionResult> Update(decimal id, [FromBody] UserRoleDTO userRoleDTO)
        {
            if (id != userRoleDTO.Id)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await userRoleRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var userRole = UserRoleDTO.ConvertTo(userRoleDTO);

            try
            {
                await userRoleRepository.UpdateAsync(userRole);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar la asignación de rol {Id}", id);
                return BadRequest("No se pudo actualizar la asignación de rol.");
            }

            return NoContent();
        }

        // DELETE /UserRole/5
        [HttpDelete("{id:decimal}")]
        public async Task<IActionResult> Delete(decimal id)
        {
            var userRole = await userRoleRepository.FindAsync(id);
            if (userRole is null)
                return NotFound();

            try
            {
                await userRoleRepository.DeleteAsync(userRole);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar la asignación de rol {Id}", id);
                return BadRequest("No se pudo eliminar la asignación de rol.");
            }

            return NoContent();
        }
    }
}
