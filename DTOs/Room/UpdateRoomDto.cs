namespace BookingApi.DTOs.Room
{
    public class UpdateRoomDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal PricePerNight { get; set; }
    }
}
