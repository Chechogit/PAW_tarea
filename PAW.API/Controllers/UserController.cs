using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ILogger<UserController> logger, IUserRepository userRepository) : ControllerBase
    {
        // GET /User?page=1&pageSize=25
        [HttpGet(Name = "GetUsers")]
        public async Task<PagedResult<UserDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await userRepository.ReadPagedAsync(page, pageSize);
            return result.Map(UserDTO.ConvertFrom);
        }

        // GET /User/5
        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user = await userRepository.FindAsync(id);
            if (user is null)
                return NotFound();

            return UserDTO.ConvertFrom(user);
        }

        // POST /User
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Create([FromBody] UserDTO userDTO)
        {
            var user = UserDTO.ConvertTo(userDTO);
            user.UserId = 0;                          // el Id lo genera SQL Server (IDENTITY)
            user.CreatedAt = DateTime.Now;
            user.LastModified = DateTime.Now;

            try
            {
                await userRepository.CreateAsync(user);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear el usuario");
                return BadRequest("No se pudo crear el usuario.");
            }

            return CreatedAtRoute("GetUserById", new { id = user.UserId }, UserDTO.ConvertFrom(user));
        }

        // PUT /User/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO userDTO)
        {
            if (id != userDTO.UserId)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await userRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var user = UserDTO.ConvertTo(userDTO);
            user.CreatedAt = existing.CreatedAt;           // la fecha de creación no cambia al editar
            user.LastModified = DateTime.Now;

            try
            {
                await userRepository.UpdateAsync(user);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar el usuario {Id}", id);
                return BadRequest("No se pudo actualizar el usuario.");
            }

            return NoContent();
        }

        // DELETE /User/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userRepository.FindAsync(id);
            if (user is null)
                return NotFound();

            try
            {
                await userRepository.DeleteAsync(user);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar el usuario {Id}", id);
                return BadRequest("No se pudo eliminar el usuario.");
            }

            return NoContent();
        }
    }
}
