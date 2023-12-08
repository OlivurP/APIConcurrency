using System.ComponentModel.DataAnnotations;

namespace RazorConcurrency.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Movie? Movie { get; set; }
        public Seat? Seat { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
