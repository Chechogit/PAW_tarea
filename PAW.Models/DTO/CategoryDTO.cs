using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class CategoryDTO
{
    [JsonPropertyName("categoryId")]
    [Display(Name = "Category ID")]
    public int CategoryId { get; set; }

    [JsonPropertyName("categoryName")]
    [Display(Name = "Name")]
    [Required(ErrorMessage = "The name is required.")]
    [StringLength(255)]
    public string? CategoryName { get; set; }

    [JsonPropertyName("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [JsonPropertyName("lastModified")]
    [Display(Name = "Last Modified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("modifiedBy")]
    [Display(Name = "Modified By")]
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    public static CategoryDTO ConvertFrom(Category category)
    {
        return new CategoryDTO
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description,
            LastModified = category.LastModified,
            ModifiedBy = category.ModifiedBy
        };
    }

    public static Category ConvertTo(CategoryDTO categoryDTO)
    {
        return new Category
        {
            CategoryId = categoryDTO.CategoryId,
            CategoryName = categoryDTO.CategoryName,
            Description = categoryDTO.Description,
            LastModified = categoryDTO.LastModified,
            ModifiedBy = categoryDTO.ModifiedBy
        };
    }
}