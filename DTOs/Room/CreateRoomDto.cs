namespace BookingApi.DTOs.Room
{
    public class CreateRoomDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PricePerNight { get; set; }
    }
}
