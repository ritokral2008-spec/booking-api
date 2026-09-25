using BookingApi.Data;
using BookingApi.Exceptions;
using BookingApi.Models;
using BookingApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories
{
    public class RoomRepository: IRoomRepository
    {
        private readonly AppDbContext _context;
        public RoomRepository(
            AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(Room room)
        {
            await _context.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Room>, int totalCount)> GetAll(
            int page,
            int pageSize,
            int? pricePerNight,
            string? sortBy,
            bool sortDescending)
        {
            var query = _context.Rooms
                .AsQueryable();

            if(pricePerNight.HasValue)
            {
                query = query.Where(r => r.PricePerNight >= pricePerNight);
            }

            switch(sortBy?.ToLower())
            {
                case "pricepernight":
                    query = sortDescending
                        ? query.OrderByDescending(r => r.PricePerNight)
                        .ThenByDescending(r => r.Id)
                        : query.OrderBy(r => r.PricePerNight)
                        .ThenBy(r => r.Id);
                break;

                default:
                    query = sortDescending
                        ? query.OrderByDescending(r => r.Id)
                        : query.OrderBy(r => r.Id);
                break;
            }

            var totalCount = await query.CountAsync();

            var rooms = await query
                .Include(r => r.Bookings)
                .ThenInclude(b => b.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (rooms, totalCount);
        }

        public async Task<Room> GetByIdWithBookings(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Bookings)
                .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if(room == null)
                throw new RoomNotFoundException("Комната не найдена");

            return room;
        }
        public async Task<Room> GetById(int id)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);

            if(room == null)
                throw new RoomNotFoundException("Комната не найдена");

            return room;
        }

        public async Task Remove(int id)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);

            if(room == null)
                throw new RoomNotFoundException("Комната не найдена");

            _context.Remove(room);
            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
