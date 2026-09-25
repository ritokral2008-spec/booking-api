using BookingApi.DTOs.Booking;
using BookingApi.Models;

namespace BookingApi.Mappers
{
    public class BookingMapper
    {
        public static ResponseBookingDto ToDto(Booking booking)
        {
            return new ResponseBookingDto
            {
                Id = booking.Id,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                Status = booking.Status,
                TotalPrice = booking.TotalPrice
            };
        }
    }
}
