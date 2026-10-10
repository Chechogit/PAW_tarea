using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class InventoryDTO
{
    [JsonPropertyName("inventoryId")]
    [Display(Name = "Inventory ID")]
    public int InventoryId { get; set; }

    [JsonPropertyName("unitPrice")]
    [Display(Name = "Unit Price")]
    [Range(0, 99999999.99, ErrorMessage = "The price must be a positive number.")]
    public decimal? UnitPrice { get; set; }

    [JsonPropertyName("unitsInStock")]
    [Display(Name = "Units In Stock")]
    [Range(0, int.MaxValue, ErrorMessage = "The stock cannot be negative.")]
    public int? UnitsInStock { get; set; }

    [JsonPropertyName("productId")]
    [Display(Name = "Product ID")]
    public int? ProductId { get; set; }

    [JsonPropertyName("dateAdded")]
    [Display(Name = "Date Added")]
    public DateTime? DateAdded { get; set; }

    [JsonPropertyName("lastUpdated")]
    [Display(Name = "Last Updated")]
    public DateTime? LastUpdated { get; set; }

    [JsonPropertyName("modifiedBy")]
    [Display(Name = "Modified By")]
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    public static InventoryDTO ConvertFrom(Inventory inventory)
    {
        return new InventoryDTO
        {
            InventoryId = inventory.InventoryId,
            UnitPrice = inventory.UnitPrice,
            UnitsInStock = inventory.UnitsInStock,
            ProductId = inventory.ProductId,
            DateAdded = inventory.DateAdded,
            LastUpdated = inventory.LastUpdated,
            ModifiedBy = inventory.ModifiedBy
        };
    }

    public static Inventory ConvertTo(InventoryDTO inventoryDTO)
    {
        return new Inventory
        {
            InventoryId = inventoryDTO.InventoryId,
            UnitPrice = inventoryDTO.UnitPrice,
            UnitsInStock = inventoryDTO.UnitsInStock,
            ProductId = inventoryDTO.ProductId,
            DateAdded = inventoryDTO.DateAdded,
            LastUpdated = inventoryDTO.LastUpdated,
            ModifiedBy = inventoryDTO.ModifiedBy
        };
    }
}
