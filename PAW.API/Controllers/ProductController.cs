using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(ILogger<ProductController> logger, IProductRepository productRepository) : ControllerBase
    {
        [HttpGet(Name = "GetProducts")]
        public async Task<IEnumerable<ProductDTO>> GetAll()
        {
            var products = await productRepository.ReadAsync();
            return products.Select(ProductDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await productRepository.FindAsync(id);
            if (product is null)
                return NotFound();

            return ProductDTO.ConvertFrom(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create([FromBody] ProductDTO productDTO)
        {
            var product = ProductDTO.ConvertTo(productDTO);
            product.ProductId = 0;
            product.LastModified = DateTime.Now;

            try
            {
                await productRepository.CreateAsync(product);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear el producto");
                return BadRequest("No se pudo crear el producto. Verifique que la categoría, el inventario y el proveedor existan.");
            }

            return CreatedAtRoute("GetProductById", new { id = product.ProductId }, ProductDTO.ConvertFrom(product));
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
                return BadRequest("El Id de la URL no coincide con el del producto.");

            var existing = await productRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var product = ProductDTO.ConvertTo(productDTO);
            product.CreatedBy = existing.CreatedBy;
            product.LastModified = DateTime.Now;

            try
            {
                await productRepository.UpdateAsync(product);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar el producto {Id}", id);
                return BadRequest("No se pudo actualizar el producto. Verifique que la categoría, el inventario y el proveedor existan.");
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await productRepository.FindAsync(id);
            if (product is null)
                return NotFound();

            try
            {
                await productRepository.DeleteAsync(product);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar el producto {Id}", id);
                return BadRequest("No se pudo eliminar el producto.");
            }

            return NoContent();
        }
    }
}