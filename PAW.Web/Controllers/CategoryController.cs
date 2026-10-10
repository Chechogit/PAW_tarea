using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(
            ICategoryService categoryService,
            IExchangeRateService exchangeRateService,
            ILogger<CategoryController> logger)
            : base(exchangeRateService)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        // GET: /Category?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new CategoryViewModel
            {
                Categories = await _categoryService.GetCategoriesAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /Category/Details/5  -> partial para el modal (la MISMA que usa el detalle de Product)
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if (category is null)
                return NotFound();

            return PartialView("_CategoryDetails", category);
        }

        // GET: /Category/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryDTO());
        }

        // POST: /Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return View(category);

            try
            {
                var created = await _categoryService.CreateCategoryAsync(category);
                TempData["Success"] = $"Category \"{created.CategoryName}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating category");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(category);
            }
        }

        // GET: /Category/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if (category is null)
                return NotFound();

            return View(category);
        }

        // POST: /Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] CategoryDTO category)
        {
            if (!ModelState.IsValid)
                return View(category);

            try
            {
                await _categoryService.UpdateCategoryAsync(category);
                TempData["Success"] = $"Category \"{category.CategoryName}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating category {Id}", category.CategoryId);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(category);
            }
        }

        // GET: /Category/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if (category is null)
                return NotFound();

            return View(category);
        }

        // POST: /Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                TempData["Success"] = $"Category #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting category {Id}", id);

                var category = await _categoryService.GetCategoryAsync(id);
                if (category is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(category);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
