using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class NotificationDTO
{
    [JsonPropertyName("id")]
    [Display(Name = "ID")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    [Display(Name = "User ID")]
    [Range(1, int.MaxValue, ErrorMessage = "The user ID is required.")]
    public int UserId { get; set; }

    [JsonPropertyName("message")]
    [Display(Name = "Message")]
    [Required(ErrorMessage = "The message is required.")]
    [StringLength(255)]
    public string? Message { get; set; }

    [JsonPropertyName("isRead")]
    [Display(Name = "Read")]
    public bool IsRead { get; set; }

    [JsonPropertyName("createdAt")]
    [Display(Name = "Created At")]
    public DateTime? CreatedAt { get; set; }

    public static NotificationDTO ConvertFrom(Notification notification)
    {
        return new NotificationDTO
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            IsRead = notification.IsRead ?? false,
            CreatedAt = notification.CreatedAt
        };
    }

    public static Notification ConvertTo(NotificationDTO notificationDTO)
    {
        return new Notification
        {
            Id = notificationDTO.Id,
            UserId = notificationDTO.UserId,
            Message = notificationDTO.Message ?? string.Empty,
            IsRead = notificationDTO.IsRead,
            CreatedAt = notificationDTO.CreatedAt
        };
    }
}
