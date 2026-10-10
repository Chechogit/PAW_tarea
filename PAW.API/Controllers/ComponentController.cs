using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController(ILogger<ComponentController> logger, IComponentRepository componentRepository) : ControllerBase
    {
        // GET /Component?page=1&pageSize=25
        [HttpGet(Name = "GetComponents")]
        public async Task<PagedResult<ComponentDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await componentRepository.ReadPagedAsync(page, pageSize);
            return result.Map(ComponentDTO.ConvertFrom);
        }

        // GET /Component/5
        [HttpGet("{id:decimal}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>> GetById(decimal id)
        {
            var component = await componentRepository.FindAsync(id);
            if (component is null)
                return NotFound();

            return ComponentDTO.ConvertFrom(component);
        }

        // POST /Component
        [HttpPost]
        public async Task<ActionResult<ComponentDTO>> Create([FromBody] ComponentDTO componentDTO)
        {
            var component = ComponentDTO.ConvertTo(componentDTO);
            component.Id = 0m;                          // el Id lo genera SQL Server (IDENTITY)

            try
            {
                await componentRepository.CreateAsync(component);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear el componente");
                return BadRequest("No se pudo crear el componente.");
            }

            return CreatedAtRoute("GetComponentById", new { id = component.Id }, ComponentDTO.ConvertFrom(component));
        }

        // PUT /Component/5
        [HttpPut("{id:decimal}")]
        public async Task<IActionResult> Update(decimal id, [FromBody] ComponentDTO componentDTO)
        {
            if (id != componentDTO.Id)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await componentRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var component = ComponentDTO.ConvertTo(componentDTO);

            try
            {
                await componentRepository.UpdateAsync(component);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar el componente {Id}", id);
                return BadRequest("No se pudo actualizar el componente.");
            }

            return NoContent();
        }

        // DELETE /Component/5
        [HttpDelete("{id:decimal}")]
        public async Task<IActionResult> Delete(decimal id)
        {
            var component = await componentRepository.FindAsync(id);
            if (component is null)
                return NotFound();

            try
            {
                await componentRepository.DeleteAsync(component);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar el componente {Id}", id);
                return BadRequest("No se pudo eliminar el componente.");
            }

            return NoContent();
        }
    }
}
