using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController(ILogger<InventoryController> logger, IInventoryRepository inventoryRepository) : ControllerBase
    {
        // GET /Inventory?page=1&pageSize=25
        [HttpGet(Name = "GetInventories")]
        public async Task<PagedResult<InventoryDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await inventoryRepository.ReadPagedAsync(page, pageSize);
            return result.Map(InventoryDTO.ConvertFrom);
        }

        // GET /Inventory/5
        [HttpGet("{id:int}", Name = "GetInventoryById")]
        public async Task<ActionResult<InventoryDTO>> GetById(int id)
        {
            var inventory = await inventoryRepository.FindAsync(id);
            if (inventory is null)
                return NotFound();

            return InventoryDTO.ConvertFrom(inventory);
        }

        // POST /Inventory
        [HttpPost]
        public async Task<ActionResult<InventoryDTO>> Create([FromBody] InventoryDTO inventoryDTO)
        {
            var inventory = InventoryDTO.ConvertTo(inventoryDTO);
            inventory.InventoryId = 0;                          // el Id lo genera SQL Server (IDENTITY)
            inventory.DateAdded = DateTime.Now;
            inventory.LastUpdated = DateTime.Now;

            try
            {
                await inventoryRepository.CreateAsync(inventory);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear el inventario");
                return BadRequest("No se pudo crear el inventario.");
            }

            return CreatedAtRoute("GetInventoryById", new { id = inventory.InventoryId }, InventoryDTO.ConvertFrom(inventory));
        }

        // PUT /Inventory/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] InventoryDTO inventoryDTO)
        {
            if (id != inventoryDTO.InventoryId)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await inventoryRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var inventory = InventoryDTO.ConvertTo(inventoryDTO);
            inventory.DateAdded = existing.DateAdded;           // la fecha de ingreso no cambia al editar
            inventory.LastUpdated = DateTime.Now;

            try
            {
                await inventoryRepository.UpdateAsync(inventory);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar el inventario {Id}", id);
                return BadRequest("No se pudo actualizar el inventario.");
            }

            return NoContent();
        }

        // DELETE /Inventory/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await inventoryRepository.FindAsync(id);
            if (inventory is null)
                return NotFound();

            try
            {
                await inventoryRepository.DeleteAsync(inventory);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar el inventario {Id}", id);
                return BadRequest("No se puede eliminar el inventario porque tiene productos asociados.");
            }

            return NoContent();
        }
    }
}
