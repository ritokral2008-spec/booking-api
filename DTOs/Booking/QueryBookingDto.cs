using BookingApi.Models.Enums;

namespace BookingApi.DTOs.Booking
{
    public class QueryBookingDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? UserId { get; set; }
        public int? RoomId { get; set; }
        public BookingStatus? Status { get; set; }
        public DateTime? CheckInFrom { get; set; }
        public DateTime? CheckInTo { get; set; }
        public string SortBy { get; set; } = "id";
        public bool SortDescending { get; set; } = false;
    }
}
