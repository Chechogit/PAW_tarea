using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(ILogger<RoleController> logger, IRoleRepository roleRepository) : ControllerBase
    {
        // GET /Role?page=1&pageSize=25
        [HttpGet(Name = "GetRoles")]
        public async Task<PagedResult<RoleDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await roleRepository.ReadPagedAsync(page, pageSize);
            return result.Map(RoleDTO.ConvertFrom);
        }

        // GET /Role/5
        [HttpGet("{id:int}", Name = "GetRoleById")]
        public async Task<ActionResult<RoleDTO>> GetById(int id)
        {
            var role = await roleRepository.FindAsync(id);
            if (role is null)
                return NotFound();

            return RoleDTO.ConvertFrom(role);
        }

        // POST /Role
        [HttpPost]
        public async Task<ActionResult<RoleDTO>> Create([FromBody] RoleDTO roleDTO)
        {
            var role = RoleDTO.ConvertTo(roleDTO);
            role.RoleId = 0;                          // el Id lo genera SQL Server (IDENTITY)

            try
            {
                await roleRepository.CreateAsync(role);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear el rol");
                return BadRequest("No se pudo crear el rol.");
            }

            return CreatedAtRoute("GetRoleById", new { id = role.RoleId }, RoleDTO.ConvertFrom(role));
        }

        // PUT /Role/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleDTO roleDTO)
        {
            if (id != roleDTO.RoleId)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await roleRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var role = RoleDTO.ConvertTo(roleDTO);

            try
            {
                await roleRepository.UpdateAsync(role);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar el rol {Id}", id);
                return BadRequest("No se pudo actualizar el rol.");
            }

            return NoContent();
        }

        // DELETE /Role/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await roleRepository.FindAsync(id);
            if (role is null)
                return NotFound();

            try
            {
                await roleRepository.DeleteAsync(role);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar el rol {Id}", id);
                return BadRequest("No se pudo eliminar el rol.");
            }

            return NoContent();
        }
    }
}
