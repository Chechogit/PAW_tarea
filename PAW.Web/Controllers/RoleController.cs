using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(
            IRoleService roleService,
            IExchangeRateService exchangeRateService,
            ILogger<RoleController> logger)
            : base(exchangeRateService)
        {
            _roleService = roleService;
            _logger = logger;
        }

        // GET: /Role?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new RoleViewModel
            {
                Roles = await _roleService.GetRolesAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /Role/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var role = await _roleService.GetRoleAsync(id);
            if (role is null)
                return NotFound();

            return PartialView("_RoleDetails", role);
        }

        // GET: /Role/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new RoleDTO());
        }

        // POST: /Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] RoleDTO role)
        {
            if (!ModelState.IsValid)
                return View(role);

            try
            {
                var created = await _roleService.CreateRoleAsync(role);
                TempData["Success"] = $"Role \"{created.RoleName}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating role");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(role);
            }
        }

        // GET: /Role/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleService.GetRoleAsync(id);
            if (role is null)
                return NotFound();

            return View(role);
        }

        // POST: /Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] RoleDTO role)
        {
            if (!ModelState.IsValid)
                return View(role);

            try
            {
                await _roleService.UpdateRoleAsync(role);
                TempData["Success"] = $"Role \"{role.RoleName}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating role {Id}", role.RoleId);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(role);
            }
        }

        // GET: /Role/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _roleService.GetRoleAsync(id);
            if (role is null)
                return NotFound();

            return View(role);
        }

        // POST: /Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                TempData["Success"] = $"Role #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting role {Id}", id);

                var role = await _roleService.GetRoleAsync(id);
                if (role is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(role);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
