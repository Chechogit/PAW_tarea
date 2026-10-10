using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserActionDTO
{
    [JsonPropertyName("id")]
    [Display(Name = "ID")]
    public decimal Id { get; set; }

    [JsonPropertyName("name")]
    [Display(Name = "Name")]
    [Required(ErrorMessage = "The name is required.")]
    [StringLength(50)]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    [Display(Name = "Description")]
    [StringLength(100)]
    public string? Description { get; set; }

    public static UserActionDTO ConvertFrom(UserAction userAction)
    {
        return new UserActionDTO
        {
            Id = userAction.Id,
            Name = userAction.Name,
            Description = userAction.Description
        };
    }

    public static UserAction ConvertTo(UserActionDTO userActionDTO)
    {
        return new UserAction
        {
            Id = userActionDTO.Id,
            Name = userActionDTO.Name,
            Description = userActionDTO.Description
        };
    }
}
