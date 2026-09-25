using BookingApi.DTOs;
using BookingApi.DTOs.Booking;
using BookingApi.DTOs.Room;

namespace BookingApi.Services.Interfaces
{
    public interface IRoomService
    {
        Task<ResponseRoomDto> Add(CreateRoomDto dto);
        Task<PagedResponse<ResponseRoomDto>> GetAll(QueryRoomDto query);
        Task<ResponseRoomDto> GetById(int id);
        Task<ResponseRoomDto> Update(int id, UpdateRoomDto dto);
        Task Remove(int id);
    }
}
