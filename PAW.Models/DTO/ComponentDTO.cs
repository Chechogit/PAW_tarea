using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ComponentDTO
{
    [JsonPropertyName("id")]
    [Display(Name = "ID")]
    public decimal Id { get; set; }

    [JsonPropertyName("name")]
    [Display(Name = "Name")]
    [Required(ErrorMessage = "The name is required.")]
    [StringLength(50)]
    public string? Name { get; set; }

    [JsonPropertyName("content")]
    [Display(Name = "Content")]
    [Required(ErrorMessage = "The content is required.")]
    [StringLength(100)]
    public string? Content { get; set; }

    public static ComponentDTO ConvertFrom(Component component)
    {
        return new ComponentDTO
        {
            Id = component.Id,
            Name = component.Name,
            Content = component.Content
        };
    }

    public static Component ConvertTo(ComponentDTO componentDTO)
    {
        return new Component
        {
            Id = componentDTO.Id,
            Name = componentDTO.Name ?? string.Empty,
            Content = componentDTO.Content ?? string.Empty
        };
    }
}
