using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserDTO
{
    [JsonPropertyName("userId")]
    [Display(Name = "User ID")]
    public int UserId { get; set; }

    [JsonPropertyName("username")]
    [Display(Name = "Username")]
    [Required(ErrorMessage = "The username is required.")]
    [StringLength(255)]
    public string? Username { get; set; }

    [JsonPropertyName("email")]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "The email is not valid.")]
    [StringLength(255)]
    public string? Email { get; set; }

    [JsonPropertyName("passwordHash")]
    [Display(Name = "Password Hash")]
    [StringLength(255)]
    public string? PasswordHash { get; set; }

    [JsonPropertyName("roleId")]
    [Display(Name = "Role ID")]
    public int? RoleId { get; set; }

    [JsonPropertyName("isActive")]
    [Display(Name = "Active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("createdAt")]
    [Display(Name = "Created At")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("lastModified")]
    [Display(Name = "Modified Date")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("modifiedBy")]
    [Display(Name = "Modified By")]
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    [JsonPropertyName("lastModifiedBy")]
    [Display(Name = "Last Modified By")]
    [StringLength(50)]
    public string? LastModifiedBy { get; set; }

    public static UserDTO ConvertFrom(User user)
    {
        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            RoleId = user.RoleId,
            IsActive = user.IsActive ?? false,
            CreatedAt = user.CreatedAt,
            LastModified = user.LastModified,
            ModifiedBy = user.ModifiedBy,
            LastModifiedBy = user.LastModifiedBy
        };
    }

    public static User ConvertTo(UserDTO userDTO)
    {
        return new User
        {
            UserId = userDTO.UserId,
            Username = userDTO.Username,
            Email = userDTO.Email,
            PasswordHash = userDTO.PasswordHash,
            RoleId = userDTO.RoleId,
            IsActive = userDTO.IsActive,
            CreatedAt = userDTO.CreatedAt,
            LastModified = userDTO.LastModified,
            ModifiedBy = userDTO.ModifiedBy,
            LastModifiedBy = userDTO.LastModifiedBy
        };
    }
}
