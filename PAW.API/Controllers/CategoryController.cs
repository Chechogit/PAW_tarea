using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository) : ControllerBase
    {
        // GET /Category?page=1&pageSize=25
        [HttpGet(Name = "GetCategories")]
        public async Task<PagedResult<CategoryDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await categoryRepository.ReadPagedAsync(page, pageSize);
            return result.Map(CategoryDTO.ConvertFrom);
        }

        // GET /Category/5
        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await categoryRepository.FindAsync(id);
            if (category is null)
                return NotFound();

            return CategoryDTO.ConvertFrom(category);
        }

        // POST /Category
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Create([FromBody] CategoryDTO categoryDTO)
        {
            var category = CategoryDTO.ConvertTo(categoryDTO);
            category.CategoryId = 0;
            category.LastModified = DateTime.Now;

            try
            {
                await categoryRepository.CreateAsync(category);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear la categoría");
                return BadRequest("No se pudo crear la categoría.");
            }

            return CreatedAtRoute("GetCategoryById", new { id = category.CategoryId }, CategoryDTO.ConvertFrom(category));
        }

        // PUT /Category/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.CategoryId)
                return BadRequest("El Id de la URL no coincide con el de la categoría.");

            var existing = await categoryRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var category = CategoryDTO.ConvertTo(categoryDTO);
            category.LastModified = DateTime.Now;

            try
            {
                await categoryRepository.UpdateAsync(category);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar la categoría {Id}", id);
                return BadRequest("No se pudo actualizar la categoría.");
            }

            return NoContent();
        }

        // DELETE /Category/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryRepository.FindAsync(id);
            if (category is null)
                return NotFound();

            try
            {
                await categoryRepository.DeleteAsync(category);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar la categoría {Id}", id);
                return BadRequest("No se puede eliminar la categoría porque tiene productos asociados.");
            }

            return NoContent();
        }
    }
}
