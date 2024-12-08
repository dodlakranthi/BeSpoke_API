using System.Drawing;

namespace BeSpokedBikesAPI.Models
{
    public class GetSalesResponse
    {
        public int SaleId { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal SalePrice { get; set; }
        public decimal DiscountedSalePrice { get; set; }
        public string SalesPerson { get; set; }
        public decimal SalesPersonCommission { get; set; }
    }
}
