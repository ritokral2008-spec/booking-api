using BookingApi.Data;
using BookingApi.Exceptions;
using BookingApi.Models;
using BookingApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(
            AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Room)
                .ToListAsync();

            return users;
        }

        public async Task<User> GetByIdWithBookings(int id)
        {
            var user = await _context.Users
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Room)
                .FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
                throw new UserNotFoundException("Пользователь не найден");

            return user;
        }
        public async Task<User> GetById(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
                throw new UserNotFoundException("Пользователь не найден");

            return user;
        }
        public async Task<User?> GetByUsername(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }
        public async Task<User?> GetByEmail(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task Remove(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if(user == null)
                throw new UserNotFoundException("Пользователь не найден");

            _context.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        
    }
}
