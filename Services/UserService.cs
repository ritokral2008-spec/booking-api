using BookingApi.DTOs.User;
using BookingApi.Exceptions;
using BookingApi.Mappers;
using BookingApi.Models;
using BookingApi.Repositories.Interfaces;
using BookingApi.Services.Interfaces;
using System.Xml;

namespace BookingApi.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        public UserService(
            IUserRepository userRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }
        public async Task<ResponseUserDto> Add(CreateUserDto dto)
        {
            var existingUsername = await _userRepository.GetByUsername(dto.Username);

            if(existingUsername != null)
                throw new UsernameAlreadyExistsException(
                    "Пользователь с таким Username уже существует");

            var existingEmail = await _userRepository.GetByEmail(dto.Email);

            if(existingEmail != null)
                throw new EmailAlreadyExistsException(
                    "Пользователь с таким Email уже существует");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = passwordHash,
                Email = dto.Email,
                Role = "Admin"
            };

            await _userRepository.Add(user);

            return UserMapper.ToDto(user);
        }

        public async Task<IEnumerable<ResponseUserDto>> GetAll()
        {
            var users = await _userRepository.GetAll();

            return users
                .Select(UserMapper.ToDto)
                .ToList();
        }

        public async Task<ResponseUserDto> GetById(int id)
        {
            var user = await _userRepository.GetByIdWithBookings(id);

            return UserMapper.ToDto(user);
        }

        public async Task Remove(int id)
        {
            await _userRepository.Remove(id);
        }

        public async Task<ResponseUserDto> Update(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetById(id);

            user.Username = dto.Username;
            user.Email = dto.Email;

            user.PasswordHash = _passwordService.HashPassword(
                user,
                dto.Password);

            await _userRepository.Save();

            return UserMapper.ToDto(user);
        }
    }
}
