using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BeSpokedBikesAPI.Models
{
    public class Products
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string Manufacturer { get; set; }
        public string? Style { get; set; }
        public decimal PurchasePrice    { get; set; }
        public decimal SalePrice { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal CommissionPercentage { get; set; }

        [JsonIgnore]
        public List<Discounts> Discounts { get; set; }
        [JsonIgnore]
        public List<Sales> Sales { get; set; }
    }

    public class UpdateProductRequest
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required string Manufacturer { get; set; }
        public string? Style { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int QuantityOnHand { get; set; }
        public decimal CommissionPercentage { get; set; }
    }
}
