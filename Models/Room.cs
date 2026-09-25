namespace BookingApi.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
