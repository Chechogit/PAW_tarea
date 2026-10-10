using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(
            IUserRoleService userRoleService,
            IExchangeRateService exchangeRateService,
            ILogger<UserRoleController> logger)
            : base(exchangeRateService)
        {
            _userRoleService = userRoleService;
            _logger = logger;
        }

        // GET: /UserRole?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new UserRoleViewModel
            {
                UserRoles = await _userRoleService.GetUserRolesAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /UserRole/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(decimal id)
        {
            var userRole = await _userRoleService.GetUserRoleAsync(id);
            if (userRole is null)
                return NotFound();

            return PartialView("_UserRoleDetails", userRole);
        }

        // GET: /UserRole/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserRoleDTO());
        }

        // POST: /UserRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserRoleDTO userRole)
        {
            if (!ModelState.IsValid)
                return View(userRole);

            try
            {
                var created = await _userRoleService.CreateUserRoleAsync(userRole);
                TempData["Success"] = $"User Role #{created.Id} was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating userRole");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(userRole);
            }
        }

        // GET: /UserRole/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(decimal id)
        {
            var userRole = await _userRoleService.GetUserRoleAsync(id);
            if (userRole is null)
                return NotFound();

            return View(userRole);
        }

        // POST: /UserRole/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] UserRoleDTO userRole)
        {
            if (!ModelState.IsValid)
                return View(userRole);

            try
            {
                await _userRoleService.UpdateUserRoleAsync(userRole);
                TempData["Success"] = $"User Role #{userRole.Id} was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating userRole {Id}", userRole.Id);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(userRole);
            }
        }

        // GET: /UserRole/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(decimal id)
        {
            var userRole = await _userRoleService.GetUserRoleAsync(id);
            if (userRole is null)
                return NotFound();

            return View(userRole);
        }

        // POST: /UserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            try
            {
                await _userRoleService.DeleteUserRoleAsync(id);
                TempData["Success"] = $"User Role #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting userRole {Id}", id);

                var userRole = await _userRoleService.GetUserRoleAsync(id);
                if (userRole is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(userRole);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
