using BookingApi.DTOs;
using BookingApi.DTOs.Booking;

namespace BookingApi.Services.Interfaces
{
    public interface IBookingService
    {
        Task<ResponseBookingDto> Create(
            CreateBookingDto dto,
            int userId);
        Task<PagedResponse<ResponseBookingDto>> GetAll(QueryBookingDto query);
        Task<ResponseBookingDto> GetById(int id);
        Task<ResponseBookingDto> Update(
            int id,
            UpdateBookingDto dto,
            int userId,
            bool isAdmin);
        Task Remove(
            int id,
            int userId,
            bool isAdmin);
        Task<ResponseBookingDto> Confirm(
            int id,
            int userId,
            bool isAdmin);
        Task<ResponseBookingDto> Cancel(
            int id,
            int userId,
            bool isAdmin);
    }
}
