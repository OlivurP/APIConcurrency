using System.ComponentModel.DataAnnotations;

namespace RazorConcurrency.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? AgeLimit { get; set; }
        public List<Order>? Orders { get; set; }
        public List<Seat>? Seats { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public Movie()
        {
            Id = 0;
        }

    }
}
