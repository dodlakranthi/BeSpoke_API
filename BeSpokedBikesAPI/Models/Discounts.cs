using System.ComponentModel.DataAnnotations.Schema;

namespace BeSpokedBikesAPI.Models
{
    public class Discounts
    {
        public int DiscountId { get; set; }
        public int ProductId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public Products Product { get; set; }
    }
}
