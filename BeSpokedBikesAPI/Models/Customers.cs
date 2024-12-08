using System.Text.Json.Serialization;

namespace BeSpokedBikesAPI.Models
{
    public class Customers
    {
        public int CustomerId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public DateTime StartDate { get; set; }
        [JsonIgnore]
        public List<Sales> Sales { get; set; }
    }
}
