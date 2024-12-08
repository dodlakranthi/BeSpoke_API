using Microsoft.Identity.Client;

namespace BeSpokedBikesAPI.Models
{
    public class Sales
    {
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
        public int SalespersonId { get; set; }
        public SalesPersons Salesperson { get; set; }
        public int CustomerId { get; set; }
        public Customers Customer { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal DiscountedSalePrice { get; set; }
    }

    public class AddNewSaleRequest
    {
        public int ProductId { get; set; }
        public int SalespersonId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
