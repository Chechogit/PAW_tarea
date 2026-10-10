using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserActionController : ControllerBase
    {
        private readonly IUserActionService _userActionService;
        private readonly ILogger<UserActionController> _logger;

        public UserActionController(
            IUserActionService userActionService,
            IExchangeRateService exchangeRateService,
            ILogger<UserActionController> logger)
            : base(exchangeRateService)
        {
            _userActionService = userActionService;
            _logger = logger;
        }

        // GET: /UserAction?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new UserActionViewModel
            {
                UserActions = await _userActionService.GetUserActionsAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /UserAction/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(decimal id)
        {
            var userAction = await _userActionService.GetUserActionAsync(id);
            if (userAction is null)
                return NotFound();

            return PartialView("_UserActionDetails", userAction);
        }

        // GET: /UserAction/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserActionDTO());
        }

        // POST: /UserAction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserActionDTO userAction)
        {
            if (!ModelState.IsValid)
                return View(userAction);

            try
            {
                var created = await _userActionService.CreateUserActionAsync(userAction);
                TempData["Success"] = $"User Action \"{created.Name}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating userAction");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(userAction);
            }
        }

        // GET: /UserAction/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(decimal id)
        {
            var userAction = await _userActionService.GetUserActionAsync(id);
            if (userAction is null)
                return NotFound();

            return View(userAction);
        }

        // POST: /UserAction/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] UserActionDTO userAction)
        {
            if (!ModelState.IsValid)
                return View(userAction);

            try
            {
                await _userActionService.UpdateUserActionAsync(userAction);
                TempData["Success"] = $"User Action \"{userAction.Name}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating userAction {Id}", userAction.Id);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(userAction);
            }
        }

        // GET: /UserAction/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(decimal id)
        {
            var userAction = await _userActionService.GetUserActionAsync(id);
            if (userAction is null)
                return NotFound();

            return View(userAction);
        }

        // POST: /UserAction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            try
            {
                await _userActionService.DeleteUserActionAsync(id);
                TempData["Success"] = $"User Action #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting userAction {Id}", id);

                var userAction = await _userActionService.GetUserActionAsync(id);
                if (userAction is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(userAction);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
