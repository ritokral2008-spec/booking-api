using BookingApi.Models.Enums;

namespace BookingApi.DTOs.Booking
{
    public class ResponseBookingDto
    {
        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }
    }
}
