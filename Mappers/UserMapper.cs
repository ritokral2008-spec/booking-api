using BookingApi.DTOs.User;
using BookingApi.Models;

namespace BookingApi.Mappers
{
    public class UserMapper
    {
        public static ResponseUserDto ToDto(User user)
        {
            return new ResponseUserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
    }
}
