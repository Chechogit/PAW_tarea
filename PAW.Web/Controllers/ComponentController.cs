using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class ComponentController : ControllerBase
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(
            IComponentService componentService,
            IExchangeRateService exchangeRateService,
            ILogger<ComponentController> logger)
            : base(exchangeRateService)
        {
            _componentService = componentService;
            _logger = logger;
        }

        // GET: /Component?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new ComponentViewModel
            {
                Components = await _componentService.GetComponentsAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /Component/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(decimal id)
        {
            var component = await _componentService.GetComponentAsync(id);
            if (component is null)
                return NotFound();

            return PartialView("_ComponentDetails", component);
        }

        // GET: /Component/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ComponentDTO());
        }

        // POST: /Component/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ComponentDTO component)
        {
            if (!ModelState.IsValid)
                return View(component);

            try
            {
                var created = await _componentService.CreateComponentAsync(component);
                TempData["Success"] = $"Component \"{created.Name}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating component");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(component);
            }
        }

        // GET: /Component/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(decimal id)
        {
            var component = await _componentService.GetComponentAsync(id);
            if (component is null)
                return NotFound();

            return View(component);
        }

        // POST: /Component/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ComponentDTO component)
        {
            if (!ModelState.IsValid)
                return View(component);

            try
            {
                await _componentService.UpdateComponentAsync(component);
                TempData["Success"] = $"Component \"{component.Name}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating component {Id}", component.Id);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(component);
            }
        }

        // GET: /Component/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(decimal id)
        {
            var component = await _componentService.GetComponentAsync(id);
            if (component is null)
                return NotFound();

            return View(component);
        }

        // POST: /Component/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            try
            {
                await _componentService.DeleteComponentAsync(id);
                TempData["Success"] = $"Component #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting component {Id}", id);

                var component = await _componentService.GetComponentAsync(id);
                if (component is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(component);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
