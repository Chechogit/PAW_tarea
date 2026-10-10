using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class PawTaskDTO
{
    [JsonPropertyName("id")]
    [Display(Name = "ID")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    [Display(Name = "Name")]
    [Required(ErrorMessage = "The name is required.")]
    [StringLength(255)]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    [Display(Name = "Description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [JsonPropertyName("status")]
    [Display(Name = "Status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [JsonPropertyName("dueDate")]
    [Display(Name = "Due Date")]
    public DateTime? DueDate { get; set; }

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

    public static PawTaskDTO ConvertFrom(PawTask pawTask)
    {
        return new PawTaskDTO
        {
            Id = pawTask.Id,
            Name = pawTask.Name,
            Description = pawTask.Description,
            Status = pawTask.Status,
            DueDate = pawTask.DueDate,
            CreatedAt = pawTask.CreatedAt,
            LastModified = pawTask.LastModified,
            ModifiedBy = pawTask.ModifiedBy
        };
    }

    public static PawTask ConvertTo(PawTaskDTO pawTaskDTO)
    {
        return new PawTask
        {
            Id = pawTaskDTO.Id,
            Name = pawTaskDTO.Name,
            Description = pawTaskDTO.Description,
            Status = pawTaskDTO.Status,
            DueDate = pawTaskDTO.DueDate,
            CreatedAt = pawTaskDTO.CreatedAt,
            LastModified = pawTaskDTO.LastModified,
            ModifiedBy = pawTaskDTO.ModifiedBy
        };
    }
}
