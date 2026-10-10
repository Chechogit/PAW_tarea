using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class PawTaskController : ControllerBase
    {
        private readonly IPawTaskService _pawTaskService;
        private readonly ILogger<PawTaskController> _logger;

        public PawTaskController(
            IPawTaskService pawTaskService,
            IExchangeRateService exchangeRateService,
            ILogger<PawTaskController> logger)
            : base(exchangeRateService)
        {
            _pawTaskService = pawTaskService;
            _logger = logger;
        }

        // GET: /PawTask?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new PawTaskViewModel
            {
                PawTasks = await _pawTaskService.GetPawTasksAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /PawTask/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var pawTask = await _pawTaskService.GetPawTaskAsync(id);
            if (pawTask is null)
                return NotFound();

            return PartialView("_PawTaskDetails", pawTask);
        }

        // GET: /PawTask/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new PawTaskDTO());
        }

        // POST: /PawTask/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] PawTaskDTO pawTask)
        {
            if (!ModelState.IsValid)
                return View(pawTask);

            try
            {
                var created = await _pawTaskService.CreatePawTaskAsync(pawTask);
                TempData["Success"] = $"Task \"{created.Name}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating pawTask");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(pawTask);
            }
        }

        // GET: /PawTask/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var pawTask = await _pawTaskService.GetPawTaskAsync(id);
            if (pawTask is null)
                return NotFound();

            return View(pawTask);
        }

        // POST: /PawTask/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] PawTaskDTO pawTask)
        {
            if (!ModelState.IsValid)
                return View(pawTask);

            try
            {
                await _pawTaskService.UpdatePawTaskAsync(pawTask);
                TempData["Success"] = $"Task \"{pawTask.Name}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating pawTask {Id}", pawTask.Id);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(pawTask);
            }
        }

        // GET: /PawTask/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var pawTask = await _pawTaskService.GetPawTaskAsync(id);
            if (pawTask is null)
                return NotFound();

            return View(pawTask);
        }

        // POST: /PawTask/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _pawTaskService.DeletePawTaskAsync(id);
                TempData["Success"] = $"Task #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting pawTask {Id}", id);

                var pawTask = await _pawTaskService.GetPawTaskAsync(id);
                if (pawTask is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(pawTask);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
