using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly ILogger<SupplierController> _logger;

        public SupplierController(
            ISupplierService supplierService,
            IExchangeRateService exchangeRateService,
            ILogger<SupplierController> logger)
            : base(exchangeRateService)
        {
            _supplierService = supplierService;
            _logger = logger;
        }

        // GET: /Supplier?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new SupplierViewModel
            {
                Suppliers = await _supplierService.GetSuppliersAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /Supplier/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var supplier = await _supplierService.GetSupplierAsync(id);
            if (supplier is null)
                return NotFound();

            return PartialView("_SupplierDetails", supplier);
        }

        // GET: /Supplier/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new SupplierDTO());
        }

        // POST: /Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SupplierDTO supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);

            try
            {
                var created = await _supplierService.CreateSupplierAsync(supplier);
                TempData["Success"] = $"Supplier \"{created.SupplierName}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating supplier");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(supplier);
            }
        }

        // GET: /Supplier/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierService.GetSupplierAsync(id);
            if (supplier is null)
                return NotFound();

            return View(supplier);
        }

        // POST: /Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] SupplierDTO supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);

            try
            {
                await _supplierService.UpdateSupplierAsync(supplier);
                TempData["Success"] = $"Supplier \"{supplier.SupplierName}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating supplier {Id}", supplier.SupplierId);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(supplier);
            }
        }

        // GET: /Supplier/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _supplierService.GetSupplierAsync(id);
            if (supplier is null)
                return NotFound();

            return View(supplier);
        }

        // POST: /Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _supplierService.DeleteSupplierAsync(id);
                TempData["Success"] = $"Supplier #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting supplier {Id}", id);

                var supplier = await _supplierService.GetSupplierAsync(id);
                if (supplier is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(supplier);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
