using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

/// <remarks>En la tabla la columna del rol se llama RoldID (error de dedo de la BD); en el DTO se expone como RoleId.</remarks>
public class UserRoleDTO
{
    [JsonPropertyName("id")]
    [Display(Name = "ID")]
    public decimal Id { get; set; }

    [JsonPropertyName("roleId")]
    [Display(Name = "Role ID")]
    [Required(ErrorMessage = "The role ID is required.")]
    public decimal? RoleId { get; set; }

    [JsonPropertyName("userId")]
    [Display(Name = "User ID")]
    [Required(ErrorMessage = "The user ID is required.")]
    public decimal? UserId { get; set; }

    public static UserRoleDTO ConvertFrom(UserRole userRole)
    {
        return new UserRoleDTO
        {
            Id = userRole.Id,
            RoleId = userRole.RoldId,
            UserId = userRole.UserId
        };
    }

    public static UserRole ConvertTo(UserRoleDTO userRoleDTO)
    {
        return new UserRole
        {
            Id = userRoleDTO.Id,
            RoldId = userRoleDTO.RoleId,
            UserId = userRoleDTO.UserId
        };
    }
}
