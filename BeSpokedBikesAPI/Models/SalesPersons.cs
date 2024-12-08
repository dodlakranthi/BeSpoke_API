using System.Text.Json.Serialization;

namespace BeSpokedBikesAPI.Models
{
    public class SalesPersons
    {
        public int SalesPersonId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public required string Manager { get; set; }
        [JsonIgnore]
        public List<Sales> Sales { get; set; }
    }

    public class UpdateSalesPersonRequest 
    {
        public int SalesPersonId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public required string Manager { get; set; }
    }
}
