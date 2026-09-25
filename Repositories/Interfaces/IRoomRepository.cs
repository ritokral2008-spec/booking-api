using BookingApi.Models;

namespace BookingApi.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task Add(Room room);
        Task<(IEnumerable<Room>, int totalCount)> GetAll(
            int page,
            int pageSize,
            int? pricePerNight,
            string? sortBy,
            bool sortDescending);
        Task<Room> GetByIdWithBookings(int id);
        Task<Room> GetById(int id);
        Task Save();
        Task Remove(int id);
    }
}
