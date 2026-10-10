using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PawTaskController(ILogger<PawTaskController> logger, IPawTaskRepository pawTaskRepository) : ControllerBase
    {
        // GET /PawTask?page=1&pageSize=25
        [HttpGet(Name = "GetPawTasks")]
        public async Task<PagedResult<PawTaskDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await pawTaskRepository.ReadPagedAsync(page, pageSize);
            return result.Map(PawTaskDTO.ConvertFrom);
        }

        // GET /PawTask/5
        [HttpGet("{id:int}", Name = "GetPawTaskById")]
        public async Task<ActionResult<PawTaskDTO>> GetById(int id)
        {
            var pawTask = await pawTaskRepository.FindAsync(id);
            if (pawTask is null)
                return NotFound();

            return PawTaskDTO.ConvertFrom(pawTask);
        }

        // POST /PawTask
        [HttpPost]
        public async Task<ActionResult<PawTaskDTO>> Create([FromBody] PawTaskDTO pawTaskDTO)
        {
            var pawTask = PawTaskDTO.ConvertTo(pawTaskDTO);
            pawTask.Id = 0;                          // el Id lo genera SQL Server (IDENTITY)
            pawTask.CreatedAt = DateTime.Now;
            pawTask.LastModified = DateTime.Now;

            try
            {
                await pawTaskRepository.CreateAsync(pawTask);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear la tarea");
                return BadRequest("No se pudo crear la tarea.");
            }

            return CreatedAtRoute("GetPawTaskById", new { id = pawTask.Id }, PawTaskDTO.ConvertFrom(pawTask));
        }

        // PUT /PawTask/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PawTaskDTO pawTaskDTO)
        {
            if (id != pawTaskDTO.Id)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await pawTaskRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var pawTask = PawTaskDTO.ConvertTo(pawTaskDTO);
            pawTask.CreatedAt = existing.CreatedAt;           // la fecha de creación no cambia al editar
            pawTask.LastModified = DateTime.Now;

            try
            {
                await pawTaskRepository.UpdateAsync(pawTask);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar la tarea {Id}", id);
                return BadRequest("No se pudo actualizar la tarea.");
            }

            return NoContent();
        }

        // DELETE /PawTask/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pawTask = await pawTaskRepository.FindAsync(id);
            if (pawTask is null)
                return NotFound();

            try
            {
                await pawTaskRepository.DeleteAsync(pawTask);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar la tarea {Id}", id);
                return BadRequest("No se pudo eliminar la tarea.");
            }

            return NoContent();
        }
    }
}
