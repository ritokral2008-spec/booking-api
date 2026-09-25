using BookingApi.DTOs;
using BookingApi.DTOs.User;
using BookingApi.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IValidator<UpdateUserDto> _updateUserValidator;
        public UserController(
            IUserService userService,
            IValidator<CreateUserDto> createUserValidator,
            IValidator<UpdateUserDto> updateUserValidator
            )
        {
            _userService = userService;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userService.GetAll();

            return Ok(response);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var validateResult = await _createUserValidator.ValidateAsync(dto);

            if(!validateResult.IsValid)
            {
                var errors = validateResult.Errors
                    .Select(e => e.ErrorMessage);

                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Ошибка валидации",
                    Errors = errors
                });
            }

            var response = await _userService.Add(dto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _userService.GetById(id);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateUserDto dto)
        {
            var validateResponse = await _updateUserValidator.ValidateAsync(dto);

            if(!validateResponse.IsValid)
            {
                var errors = validateResponse.Errors
                    .Select(e => e.ErrorMessage);

                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Ошибка валидации",
                    Errors = errors
                });
            }

            var response = await _userService.Update(
                id,
                dto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _userService.Remove(id);

            return NoContent();
        }
    }
}
