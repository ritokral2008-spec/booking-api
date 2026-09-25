using BookingApi.DTOs.Auth;

namespace BookingApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginDto dto);
    }
}
