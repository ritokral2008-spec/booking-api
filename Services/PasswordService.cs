using BookingApi.Models;
using BookingApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BookingApi.Services
{
    public class PasswordService: IPasswordService
    {
        private readonly PasswordHasher<User> _hasher = new();
        public string HashPassword(User user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(
            User user, 
            string hashedPassword, 
            string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(
                user,
                hashedPassword,
                providedPassword);

            return result == PasswordVerificationResult.Success;
        }
    }
}
