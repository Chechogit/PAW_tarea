using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class SupplierDTO
{
    [JsonPropertyName("supplierId")]
    [Display(Name = "Supplier ID")]
    public int SupplierId { get; set; }

    [JsonPropertyName("supplierName")]
    [Display(Name = "Name")]
    [Required(ErrorMessage = "The name is required.")]
    [StringLength(255)]
    public string? SupplierName { get; set; }

    [JsonPropertyName("contactName")]
    [Display(Name = "Contact Name")]
    [StringLength(255)]
    public string? ContactName { get; set; }

    [JsonPropertyName("contactTitle")]
    [Display(Name = "Contact Title")]
    [StringLength(255)]
    public string? ContactTitle { get; set; }

    [JsonPropertyName("phone")]
    [Display(Name = "Phone")]
    [StringLength(255)]
    public string? Phone { get; set; }

    [JsonPropertyName("address")]
    [Display(Name = "Address")]
    [StringLength(1000)]
    public string? Address { get; set; }

    [JsonPropertyName("city")]
    [Display(Name = "City")]
    [StringLength(255)]
    public string? City { get; set; }

    [JsonPropertyName("country")]
    [Display(Name = "Country")]
    [StringLength(255)]
    public string? Country { get; set; }

    [JsonPropertyName("lastModified")]
    [Display(Name = "Modified Date")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("modifiedBy")]
    [Display(Name = "Modified By")]
    [StringLength(255)]
    public string? ModifiedBy { get; set; }

    public static SupplierDTO ConvertFrom(Supplier supplier)
    {
        return new SupplierDTO
        {
            SupplierId = supplier.SupplierId,
            SupplierName = supplier.SupplierName,
            ContactName = supplier.ContactName,
            ContactTitle = supplier.ContactTitle,
            Phone = supplier.Phone,
            Address = supplier.Address,
            City = supplier.City,
            Country = supplier.Country,
            LastModified = supplier.LastModified,
            ModifiedBy = supplier.ModifiedBy
        };
    }

    public static Supplier ConvertTo(SupplierDTO supplierDTO)
    {
        return new Supplier
        {
            SupplierId = supplierDTO.SupplierId,
            SupplierName = supplierDTO.SupplierName,
            ContactName = supplierDTO.ContactName,
            ContactTitle = supplierDTO.ContactTitle,
            Phone = supplierDTO.Phone,
            Address = supplierDTO.Address,
            City = supplierDTO.City,
            Country = supplierDTO.Country,
            LastModified = supplierDTO.LastModified,
            ModifiedBy = supplierDTO.ModifiedBy
        };
    }
}
