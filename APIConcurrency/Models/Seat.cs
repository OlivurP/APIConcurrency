using System.ComponentModel.DataAnnotations;

namespace RazorConcurrency.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Taken { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
