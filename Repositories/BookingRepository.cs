using BookingApi.Data;
using BookingApi.Exceptions;
using BookingApi.Models;
using BookingApi.Models.Enums;
using BookingApi.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories
{
    public class BookingRepository: IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(
            AppDbContext context)
        {
            _context = context;
        }
        public async Task Add(Booking booking)
        {
            await _context.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Booking> Items, int TotalCount)> GetAll(
            int page,
            int pageSize,
            int? userId,
            int? roomId,
            BookingStatus? status,
            DateTime? checkInFrom,
            DateTime? checkInTo,
            string? sortBy,
            bool SortDescending)
        {
            var query = _context.Bookings
                .AsQueryable();

            if(userId.HasValue)
            {
                query = query.Where(b => b.UserId == userId.Value);
            }

            if(roomId.HasValue)
            {
                query = query.Where(b => b.RoomId == roomId.Value);
            }

            if(status.HasValue)
            {
                query = query.Where(b => b.Status == status.Value);
            }

            if(checkInFrom.HasValue)
            {
                query = query.Where(b => b.CheckIn >= checkInFrom.Value);
            }

            if(checkInTo.HasValue)
            {
                query = query.Where(b => b.CheckIn <= checkInTo.Value);
            }

            switch(sortBy?.ToLower())
            {
                case "checkin":
                    query = SortDescending
                        ? query.OrderByDescending(b => b.CheckIn)
                        .ThenByDescending(b => b.Id)
                        : query.OrderBy(b => b.CheckIn)
                        .ThenBy(b => b.Id);
                break;

                case "checkout":
                    query = SortDescending
                        ? query.OrderByDescending(b => b.CheckOut)
                        .ThenByDescending(b => b.Id)
                        : query.OrderBy(b => b.CheckOut)
                        .ThenByDescending(b => b.Id);
                break;

                case "totalprice":
                    query = SortDescending
                        ? query.OrderByDescending(b => b.TotalPrice)
                        .ThenByDescending(b => b.Id)
                        : query.OrderBy(b => b.TotalPrice)
                        .ThenBy(b => b.Id);
                break;

                case "status":
                    query = SortDescending
                        ? query.OrderByDescending(b => b.Status)
                        .ThenByDescending(b => b.Id)
                        : query.OrderBy(b => b.Status)
                        .ThenBy(b => b.Id);
                break;

                default:
                    query = SortDescending
                        ? query.OrderByDescending(b => b.Id)
                        : query.OrderBy(b => b.Id);
                break;
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(b => b.Room)
                .Include(b => b.User)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        public async Task<Booking> GetById(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if(booking == null)
                throw new BookingNotFoundException("Бронирование не найдено");

            return booking;
        }

        public async Task Remove(int id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if(booking == null)
                throw new BookingNotFoundException("Бронирование не найдено");

                _context.Remove(booking);

            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> IsRoomAvailable(
            int roomId,
            DateTime checkIn, 
            DateTime checkOut,
            int? bookingId = null
            )
        {
            var hasConflict = await _context.Bookings
                .AnyAsync(b =>
                    b.RoomId == roomId &&
                    (bookingId == null || b.Id != bookingId) &&
                    b.CheckIn < checkOut &&
                    b.CheckOut > checkIn);

            return !hasConflict;
        }
    }
}
