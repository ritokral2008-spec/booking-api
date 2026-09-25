using BookingApi.DTOs;
using BookingApi.DTOs.Booking;
using BookingApi.DTOs.Room;
using BookingApi.Mappers;
using BookingApi.Models;
using BookingApi.Repositories.Interfaces;
using BookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace BookingApi.Services
{
    public class RoomService: IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(
            IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public async Task<ResponseRoomDto> Add(CreateRoomDto dto)
        {
            var room = new Room()
            {
                Title = dto.Title,
                Description = dto.Description,
                Address = dto.Address,
                PricePerNight = dto.PricePerNight
            };

            await _roomRepository.Add(room);

            return RoomMapper.ToDto(room);
        }

        public async Task<PagedResponse<ResponseRoomDto>> GetAll(QueryRoomDto query)
        {
            var rooms = await _roomRepository.GetAll(
            query.Page,
            query.PageSize,
            query.PricePerNight,
            query.SortBy,
            query.SortDescending);

            var items = rooms.Item1
                .Select(RoomMapper.ToDto)
                .ToList();

            return new PagedResponse<ResponseRoomDto>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = rooms.totalCount,
                TotalPages = (int)Math.Ceiling(
                    (double)rooms.totalCount / query.PageSize)
            };
        }

        public async Task<ResponseRoomDto> GetById(int id)
        {
            var room = await _roomRepository.GetByIdWithBookings(id);

            return RoomMapper.ToDto(room);
        }

        public async Task Remove(int id)
        {
            await _roomRepository.Remove(id);
        }

        public async Task<ResponseRoomDto> Update(int id, UpdateRoomDto dto)
        {
            var room = await _roomRepository.GetById(id);

            room.Title = dto.Title;
            room.Description = dto.Description;
            room.Address = dto.Address;
            room.PricePerNight = dto.PricePerNight;

            await _roomRepository.Save();

            return RoomMapper.ToDto(room);
        }
    }
}
