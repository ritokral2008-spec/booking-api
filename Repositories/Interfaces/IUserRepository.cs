using BookingApi.Models;

namespace BookingApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetByIdWithBookings(int id);
        Task<User> GetById(int id);
        Task<User?> GetByUsername(string username);
        Task<User?> GetByEmail(string email);
        Task Save();
        Task Remove(int id);
    }
}
