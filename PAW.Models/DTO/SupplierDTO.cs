using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class SupplierDTO
{
    [JsonPropertyName("supplierId")]
    public int SupplierId { get; set; }

    [JsonPropertyName("supplierName")]
    public string? SupplierName { get; set; }

    [JsonPropertyName("contactName")]
    public string? ContactName { get; set; }

    [JsonPropertyName("contactTitle")]
    public string? ContactTitle { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("modifiedBy")]
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