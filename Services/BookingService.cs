using BookingApi.DTOs;
using BookingApi.DTOs.Booking;
using BookingApi.Exceptions;
using BookingApi.Mappers;
using BookingApi.Models;
using BookingApi.Models.Enums;
using BookingApi.Repositories.Interfaces;
using BookingApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
using System.Text.Json;

namespace BookingApi.Services
{
    public class BookingService: IBookingService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IDistributedCache _cache;
        public BookingService(
            IUserRepository userRepository,
            IRoomRepository roomRepository,
            IBookingRepository bookingRepository,
            IDistributedCache cache)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _cache = cache;
        }
        public async Task<ResponseBookingDto> Create(
            CreateBookingDto dto,
            int userId)
        {
            await _userRepository.GetByIdWithBookings(userId);

            var room = await _roomRepository.GetByIdWithBookings(dto.RoomId);

            var isAvailable = await _bookingRepository.IsRoomAvailable(
                dto.RoomId,
                dto.CheckIn,
                dto.CheckOut);

            if(!isAvailable)
                throw new RoomAlreadyBookedException(
                    "Комната уже забронирована");

            var nights = (dto.CheckOut - dto.CheckIn).Days;

            var totalPrice = room.PricePerNight * nights;

            var booking = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                TotalPrice = totalPrice,
                Status = BookingStatus.Pending
            };

            await _bookingRepository.Add(booking);

            return BookingMapper.ToDto(booking);
        }

        public async Task<PagedResponse<ResponseBookingDto>> GetAll(QueryBookingDto query)
        {
            var bookings = await _bookingRepository.GetAll(
                query.Page,
                query.PageSize,
                query.UserId,
                query.RoomId,
                query.Status,
                query.CheckInFrom,
                query.CheckInTo,
                query.SortBy,
                query.SortDescending);

            var items = bookings.Items
                .Select(BookingMapper.ToDto)
                .ToList();

            return new PagedResponse<ResponseBookingDto>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = bookings.TotalCount,
                TotalPages = (int)Math.Ceiling(
                    (double)bookings.TotalCount / query.PageSize)
            };
        }

        public async Task<ResponseBookingDto> GetById(int id)
        {
            var cacheKey = GetCacheKey(id);

            var cachedBooking = await _cache.GetStringAsync(cacheKey);

            if(cachedBooking != null)
            {
                return JsonSerializer.Deserialize<ResponseBookingDto>(cachedBooking)!;
            }
            
            var booking = await _bookingRepository.GetById(id);

            var result = BookingMapper.ToDto(booking);

            var json = JsonSerializer.Serialize(result);

            await _cache.SetStringAsync(
                cacheKey,
                json,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });

            return result;
        }

        public async Task Remove(
            int id,
            int userId,
            bool isAdmin)
        {
            await _bookingRepository.Remove(id);

            await _cache.RemoveAsync(GetCacheKey(id));
        }

        public async Task<ResponseBookingDto> Update(
            int id,
            UpdateBookingDto dto,
            int userId,
            bool isAdmin)
        {
            var room = await _roomRepository.GetByIdWithBookings(dto.RoomId);

            var booking = await _bookingRepository.GetById(id);

            if(!isAdmin && booking.UserId != userId)
                throw new ForbiddenException(
                    "Вы не можете изменить чужое бронирование");

            if(booking.Status != BookingStatus.Pending)
                throw new InvalidBookingStatusException(
                    "Изменить бронирование можно только со статусом Pending");

            if(!await _bookingRepository.IsRoomAvailable(dto.RoomId, dto.CheckIn, dto.CheckOut, id))
                throw new RoomAlreadyBookedException(
                    "Данная комната уже забронирована");

            var nights = (dto.CheckOut - dto.CheckIn).Days;

            var totalPrice = nights * room.PricePerNight;

            booking.RoomId = dto.RoomId;
            booking.CheckIn = dto.CheckIn;
            booking.CheckOut = dto.CheckOut;
            booking.TotalPrice = totalPrice;

            await _bookingRepository.Save();

            await _cache.RemoveAsync(GetCacheKey(id));

            return BookingMapper.ToDto(booking);
        }
        public async Task<ResponseBookingDto> Confirm(
            int id,
            int userId,
            bool isAdmin)
        {
            var booking = await _bookingRepository.GetById(id);

            if(!isAdmin && booking.UserId != userId)
                throw new ForbiddenException(
                    "Невозможно подтвердить чужое бронирование");

            if(booking.Status != BookingStatus.Pending)
                throw new InvalidBookingStatusException(
                    "Можно подтвердить бронирование только со статусом Pending");

            booking.Status = BookingStatus.Confirmed;

            await _bookingRepository.Save();

            await _cache.RemoveAsync(GetCacheKey(id));

            return BookingMapper.ToDto(booking);
        }
        public async Task<ResponseBookingDto> Cancel(
            int id,
            int userId,
            bool isAdmin)
        {
            var booking = await _bookingRepository.GetById(id);

            if(!isAdmin && booking.UserId != userId)
                throw new ForbiddenException(
                    "Нельзя отменить чужое бронирование");

            if(booking.Status != BookingStatus.Pending &&
                booking.Status != BookingStatus.Confirmed)
                throw new InvalidBookingStatusException(
                    "Можно отменить бронирование только со статусом Pending или Confirmed");
            
            booking.Status = BookingStatus.Cancelled;

            await _bookingRepository.Save();

            await _cache.RemoveAsync(GetCacheKey(id));

            return BookingMapper.ToDto(booking);
        }

        private static string GetCacheKey(int id)
        {
            return $"booking:{id}";
        }
    }
}
