using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            IInventoryService inventoryService,
            IExchangeRateService exchangeRateService,
            ILogger<InventoryController> logger)
            : base(exchangeRateService)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        // GET: /Inventory?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new InventoryViewModel
            {
                Inventories = await _inventoryService.GetInventoriesAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /Inventory/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var inventory = await _inventoryService.GetInventoryAsync(id);
            if (inventory is null)
                return NotFound();

            return PartialView("_InventoryDetails", inventory);
        }

        // GET: /Inventory/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new InventoryDTO());
        }

        // POST: /Inventory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] InventoryDTO inventory)
        {
            if (!ModelState.IsValid)
                return View(inventory);

            try
            {
                var created = await _inventoryService.CreateInventoryAsync(inventory);
                TempData["Success"] = $"Inventory #{created.InventoryId} was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating inventory");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(inventory);
            }
        }

        // GET: /Inventory/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var inventory = await _inventoryService.GetInventoryAsync(id);
            if (inventory is null)
                return NotFound();

            return View(inventory);
        }

        // POST: /Inventory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] InventoryDTO inventory)
        {
            if (!ModelState.IsValid)
                return View(inventory);

            try
            {
                await _inventoryService.UpdateInventoryAsync(inventory);
                TempData["Success"] = $"Inventory #{inventory.InventoryId} was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating inventory {Id}", inventory.InventoryId);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(inventory);
            }
        }

        // GET: /Inventory/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryService.GetInventoryAsync(id);
            if (inventory is null)
                return NotFound();

            return View(inventory);
        }

        // POST: /Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _inventoryService.DeleteInventoryAsync(id);
                TempData["Success"] = $"Inventory #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting inventory {Id}", id);

                var inventory = await _inventoryService.GetInventoryAsync(id);
                if (inventory is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(inventory);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
