using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController(ILogger<SupplierController> logger, ISupplierRepository supplierRepository) : ControllerBase
    {
        // GET /Supplier?page=1&pageSize=25
        [HttpGet(Name = "GetSuppliers")]
        public async Task<PagedResult<SupplierDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await supplierRepository.ReadPagedAsync(page, pageSize);
            return result.Map(SupplierDTO.ConvertFrom);
        }

        // GET /Supplier/5
        [HttpGet("{id:int}", Name = "GetSupplierById")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);
            if (supplier is null)
                return NotFound();

            return SupplierDTO.ConvertFrom(supplier);
        }

        // POST /Supplier
        [HttpPost]
        public async Task<ActionResult<SupplierDTO>> Create([FromBody] SupplierDTO supplierDTO)
        {
            var supplier = SupplierDTO.ConvertTo(supplierDTO);
            supplier.SupplierId = 0;                          // el Id lo genera SQL Server (IDENTITY)
            supplier.LastModified = DateTime.Now;

            try
            {
                await supplierRepository.CreateAsync(supplier);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear el proveedor");
                return BadRequest("No se pudo crear el proveedor.");
            }

            return CreatedAtRoute("GetSupplierById", new { id = supplier.SupplierId }, SupplierDTO.ConvertFrom(supplier));
        }

        // PUT /Supplier/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierDTO supplierDTO)
        {
            if (id != supplierDTO.SupplierId)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await supplierRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var supplier = SupplierDTO.ConvertTo(supplierDTO);
            supplier.LastModified = DateTime.Now;

            try
            {
                await supplierRepository.UpdateAsync(supplier);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar el proveedor {Id}", id);
                return BadRequest("No se pudo actualizar el proveedor.");
            }

            return NoContent();
        }

        // DELETE /Supplier/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);
            if (supplier is null)
                return NotFound();

            try
            {
                await supplierRepository.DeleteAsync(supplier);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar el proveedor {Id}", id);
                return BadRequest("No se puede eliminar el proveedor porque tiene productos asociados.");
            }

            return NoContent();
        }
    }
}
