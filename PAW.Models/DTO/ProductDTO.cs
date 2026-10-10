using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ProductDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("name")]
    [Required(ErrorMessage = "The name is required.")]
    [StringLength(255)]
    public string? Name { get; set; }

    [JsonPropertyName("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [JsonPropertyName("rating")]
    [Range(0, 5, ErrorMessage = "The rating must be between 0 and 5.")]
    public decimal? Rating { get; set; }

    [JsonPropertyName("categoryId")]
    [Display(Name = "Category ID")]
    public int? CategoryId { get; set; }

    [JsonPropertyName("inventoryId")]
    [Display(Name = "Inventory ID")]
    public int? InventoryId { get; set; }

    [JsonPropertyName("supplierId")]
    [Display(Name = "Supplier ID")]
    public int? SupplierId { get; set; }

    [JsonPropertyName("modifiedBy")]
    [Display(Name = "Modified By")]
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    [JsonPropertyName("createdBy")]
    [Display(Name = "Created By")]
    [StringLength(100)]
    public string? CreatedBy { get; set; }

    [JsonPropertyName("comments")]
    public string? Comments { get; set; }

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    [JsonPropertyName("category")]
    public CategoryDTO? Category { get; set; }

    [JsonPropertyName("inventory")]
    public InventoryDTO? Inventory { get; set; }

    [JsonPropertyName("supplier")]
    public SupplierDTO? Supplier { get; set; }

    public static ProductDTO ConvertFrom(Product product)
    {
        return new ProductDTO
        {
            Id = Guid.NewGuid(),
            ProductId = product.ProductId,
            Name = product.ProductName,
            Description = product.Description,
            Rating = product.Rating,
            CategoryId = product.CategoryId,
            InventoryId = product.InventoryId,
            SupplierId = product.SupplierId,
            ModifiedBy = product.ModifiedBy,
            CreatedBy = product.CreatedBy,
            Comments = string.Empty,
            CreatedDate = product.LastModified ?? DateTime.Now,
            ModifiedDate = product.LastModified ?? DateTime.Now,
            Category = product.Category is null ? null : CategoryDTO.ConvertFrom(product.Category),
            Inventory = product.Inventory is null ? null : InventoryDTO.ConvertFrom(product.Inventory),
            Supplier = product.Supplier is null ? null : SupplierDTO.ConvertFrom(product.Supplier)
        };
    }

    public static Product ConvertTo(ProductDTO productDTO)
    {
        return new Product
        {
            ProductId = productDTO.ProductId,
            ProductName = productDTO.Name,
            Description = productDTO.Description,
            Rating = productDTO.Rating,
            CategoryId = productDTO.CategoryId,
            InventoryId = productDTO.InventoryId,
            SupplierId = productDTO.SupplierId,
            ModifiedBy = productDTO.ModifiedBy,
            CreatedBy = productDTO.CreatedBy,
            LastModified = productDTO.ModifiedDate
        };
    }
}