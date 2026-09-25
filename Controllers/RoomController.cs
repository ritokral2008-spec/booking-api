using BookingApi.DTOs;
using BookingApi.DTOs.Room;
using BookingApi.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController: ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IValidator<CreateRoomDto> _createRoomValidator;
        private readonly IValidator<UpdateRoomDto> _updateRoomValidator;
        public RoomController(
            IRoomService roomService,
            IValidator<CreateRoomDto> createRoomValidator,
            IValidator<UpdateRoomDto> updateRoomValidator)
        {
            _roomService = roomService;
            _createRoomValidator = createRoomValidator;
            _updateRoomValidator = updateRoomValidator;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryRoomDto query)
        {
            var response = await _roomService.GetAll(query);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateRoomDto dto)
        {
            var validationResult = await _createRoomValidator.ValidateAsync(dto);

            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage);

                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Ошибка валидации",
                    Errors = errors
                });
            }

            var response = await _roomService.Add(dto);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _roomService.GetById(id);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/update")]
        public async Task<IActionResult> Update(
            int id,
            UpdateRoomDto dto)
        {
            var validationResult = await _updateRoomValidator.ValidateAsync(dto);

            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(x => x.ErrorMessage);

                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Ошибка валидации",
                    Errors = errors
                });
            }

            var response = await _roomService.Update(
                id,
                dto);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            await _roomService.Remove(id);

            return NoContent();
        }
    }
}
