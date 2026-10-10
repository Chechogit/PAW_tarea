using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserActionController(ILogger<UserActionController> logger, IUserActionRepository userActionRepository) : ControllerBase
    {
        // GET /UserAction?page=1&pageSize=25
        [HttpGet(Name = "GetUserActions")]
        public async Task<PagedResult<UserActionDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await userActionRepository.ReadPagedAsync(page, pageSize);
            return result.Map(UserActionDTO.ConvertFrom);
        }

        // GET /UserAction/5
        [HttpGet("{id:decimal}", Name = "GetUserActionById")]
        public async Task<ActionResult<UserActionDTO>> GetById(decimal id)
        {
            var userAction = await userActionRepository.FindAsync(id);
            if (userAction is null)
                return NotFound();

            return UserActionDTO.ConvertFrom(userAction);
        }

        // POST /UserAction
        [HttpPost]
        public async Task<ActionResult<UserActionDTO>> Create([FromBody] UserActionDTO userActionDTO)
        {
            var userAction = UserActionDTO.ConvertTo(userActionDTO);
            userAction.Id = 0m;                          // el Id lo genera SQL Server (IDENTITY)

            try
            {
                await userActionRepository.CreateAsync(userAction);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear la acción de usuario");
                return BadRequest("No se pudo crear la acción de usuario.");
            }

            return CreatedAtRoute("GetUserActionById", new { id = userAction.Id }, UserActionDTO.ConvertFrom(userAction));
        }

        // PUT /UserAction/5
        [HttpPut("{id:decimal}")]
        public async Task<IActionResult> Update(decimal id, [FromBody] UserActionDTO userActionDTO)
        {
            if (id != userActionDTO.Id)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await userActionRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var userAction = UserActionDTO.ConvertTo(userActionDTO);

            try
            {
                await userActionRepository.UpdateAsync(userAction);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar la acción de usuario {Id}", id);
                return BadRequest("No se pudo actualizar la acción de usuario.");
            }

            return NoContent();
        }

        // DELETE /UserAction/5
        [HttpDelete("{id:decimal}")]
        public async Task<IActionResult> Delete(decimal id)
        {
            var userAction = await userActionRepository.FindAsync(id);
            if (userAction is null)
                return NotFound();

            try
            {
                await userActionRepository.DeleteAsync(userAction);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar la acción de usuario {Id}", id);
                return BadRequest("No se pudo eliminar la acción de usuario.");
            }

            return NoContent();
        }
    }
}
