using BookingApi.Models;
using BookingApi.Models.Enums;

namespace BookingApi.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task Add(Booking booking);
        public Task<(IEnumerable<Booking> Items, int TotalCount)> GetAll(
            int page,
            int pageSize,
            int? UserId,
            int? RoomId,
            BookingStatus? status,
            DateTime? CheckInFrom,
            DateTime? CheckInTo,
            string? sortBy,
            bool SortDescending);
        Task<Booking> GetById(int id);
        Task Save();
        Task Remove(int id);
        Task<bool> IsRoomAvailable(
            int roomId,
            DateTime checkIn, 
            DateTime checkOut,
            int? bookingId = null);
    }
}
