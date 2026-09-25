using BookingApi.DTOs.User;

namespace BookingApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseUserDto> Add(CreateUserDto dto);
        Task<IEnumerable<ResponseUserDto>> GetAll();
        Task<ResponseUserDto> GetById(int id);
        Task<ResponseUserDto> Update(int id, UpdateUserDto dto);
        Task Remove(int id);
    }
}
