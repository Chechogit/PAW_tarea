using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class RoleDTO
{
    [JsonPropertyName("roleId")]
    [Display(Name = "Role ID")]
    public int RoleId { get; set; }

    [JsonPropertyName("roleName")]
    [Display(Name = "Name")]
    [Required(ErrorMessage = "The name is required.")]
    [StringLength(255)]
    public string? RoleName { get; set; }

    public static RoleDTO ConvertFrom(Role role)
    {
        return new RoleDTO
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName
        };
    }

    public static Role ConvertTo(RoleDTO roleDTO)
    {
        return new Role
        {
            RoleId = roleDTO.RoleId,
            RoleName = roleDTO.RoleName
        };
    }
}
