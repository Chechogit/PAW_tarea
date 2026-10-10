using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            IExchangeRateService exchangeRateService,
            ILogger<UserController> logger)
            : base(exchangeRateService)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: /User?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new UserViewModel
            {
                Users = await _userService.GetUsersAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /User/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user is null)
                return NotFound();

            return PartialView("_UserDetails", user);
        }

        // GET: /User/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new UserDTO());
        }

        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserDTO user)
        {
            if (!ModelState.IsValid)
                return View(user);

            try
            {
                var created = await _userService.CreateUserAsync(user);
                TempData["Success"] = $"User \"{created.Username}\" was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating user");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(user);
            }
        }

        // GET: /User/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user is null)
                return NotFound();

            return View(user);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] UserDTO user)
        {
            if (!ModelState.IsValid)
                return View(user);

            try
            {
                await _userService.UpdateUserAsync(user);
                TempData["Success"] = $"User \"{user.Username}\" was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating user {Id}", user.UserId);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(user);
            }
        }

        // GET: /User/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user is null)
                return NotFound();

            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                TempData["Success"] = $"User #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting user {Id}", id);

                var user = await _userService.GetUserAsync(id);
                if (user is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(user);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
