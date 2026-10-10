using Microsoft.AspNetCore.Mvc;
using PAW.Models.DTO;
using PAW.Web.Models;
using PAW.Web.Services;

namespace PAW.Web.Controllers
{
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            INotificationService notificationService,
            IExchangeRateService exchangeRateService,
            ILogger<NotificationController> logger)
            : base(exchangeRateService)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        // GET: /Notification?page=2
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new NotificationViewModel
            {
                Notifications = await _notificationService.GetNotificationsAsync(page),
                TipoCambio = base.TipoCambio
            };
            return View(viewModel);
        }

        // GET: /Notification/Details/5  -> partial para el modal
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var notification = await _notificationService.GetNotificationAsync(id);
            if (notification is null)
                return NotFound();

            return PartialView("_NotificationDetails", notification);
        }

        // GET: /Notification/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new NotificationDTO());
        }

        // POST: /Notification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NotificationDTO notification)
        {
            if (!ModelState.IsValid)
                return View(notification);

            try
            {
                var created = await _notificationService.CreateNotificationAsync(notification);
                TempData["Success"] = $"Notification #{created.Id} was created.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error creating notification");
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(notification);
            }
        }

        // GET: /Notification/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var notification = await _notificationService.GetNotificationAsync(id);
            if (notification is null)
                return NotFound();

            return View(notification);
        }

        // POST: /Notification/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] NotificationDTO notification)
        {
            if (!ModelState.IsValid)
                return View(notification);

            try
            {
                await _notificationService.UpdateNotificationAsync(notification);
                TempData["Success"] = $"Notification #{notification.Id} was updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error updating notification {Id}", notification.Id);
                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(notification);
            }
        }

        // GET: /Notification/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _notificationService.GetNotificationAsync(id);
            if (notification is null)
                return NotFound();

            return View(notification);
        }

        // POST: /Notification/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _notificationService.DeleteNotificationAsync(id);
                TempData["Success"] = $"Notification #{id} was deleted.";
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Error deleting notification {Id}", id);

                var notification = await _notificationService.GetNotificationAsync(id);
                if (notification is null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, GetApiMessage(ex));
                return View(notification);
            }
        }

        private static string GetApiMessage(ApplicationException ex)
            => ex.InnerException?.Message ?? ex.Message;
    }
}
