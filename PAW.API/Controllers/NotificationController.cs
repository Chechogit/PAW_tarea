using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(ILogger<NotificationController> logger, INotificationRepository notificationRepository) : ControllerBase
    {
        // GET /Notification?page=1&pageSize=25
        [HttpGet(Name = "GetNotifications")]
        public async Task<PagedResult<NotificationDTO>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = Paging.MaxPageSize)
        {
            var result = await notificationRepository.ReadPagedAsync(page, pageSize);
            return result.Map(NotificationDTO.ConvertFrom);
        }

        // GET /Notification/5
        [HttpGet("{id:int}", Name = "GetNotificationById")]
        public async Task<ActionResult<NotificationDTO>> GetById(int id)
        {
            var notification = await notificationRepository.FindAsync(id);
            if (notification is null)
                return NotFound();

            return NotificationDTO.ConvertFrom(notification);
        }

        // POST /Notification
        [HttpPost]
        public async Task<ActionResult<NotificationDTO>> Create([FromBody] NotificationDTO notificationDTO)
        {
            var notification = NotificationDTO.ConvertTo(notificationDTO);
            notification.Id = 0;                          // el Id lo genera SQL Server (IDENTITY)
            notification.CreatedAt = DateTime.Now;

            try
            {
                await notificationRepository.CreateAsync(notification);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al crear la notificación");
                return BadRequest("No se pudo crear la notificación.");
            }

            return CreatedAtRoute("GetNotificationById", new { id = notification.Id }, NotificationDTO.ConvertFrom(notification));
        }

        // PUT /Notification/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] NotificationDTO notificationDTO)
        {
            if (id != notificationDTO.Id)
                return BadRequest("El Id de la URL no coincide con el del registro.");

            var existing = await notificationRepository.FindAsync(id);
            if (existing is null)
                return NotFound();

            var notification = NotificationDTO.ConvertTo(notificationDTO);
            notification.CreatedAt = existing.CreatedAt;           // la fecha de creación no cambia al editar

            try
            {
                await notificationRepository.UpdateAsync(notification);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al actualizar la notificación {Id}", id);
                return BadRequest("No se pudo actualizar la notificación.");
            }

            return NoContent();
        }

        // DELETE /Notification/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await notificationRepository.FindAsync(id);
            if (notification is null)
                return NotFound();

            try
            {
                await notificationRepository.DeleteAsync(notification);
            }
            catch (PAWException ex)
            {
                logger.LogError(ex, "Error al eliminar la notificación {Id}", id);
                return BadRequest("No se pudo eliminar la notificación.");
            }

            return NoContent();
        }
    }
}
