using BookingApi.DTOs;
using BookingApi.DTOs.Booking;
using BookingApi.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController: ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IValidator<CreateBookingDto> _createBookingValidator;
        private readonly IValidator<UpdateBookingDto> _updateBookingValidator;
        private readonly IValidator<QueryBookingDto> _queryBookingValidator;
        public BookingController(
            IBookingService bookingService,
            IValidator<CreateBookingDto> createBookingValidator,
            IValidator<UpdateBookingDto> updateBookingValidator,
            IValidator<QueryBookingDto> queryBookingValidator)
        {
            _bookingService = bookingService;
            _createBookingValidator = createBookingValidator;
            _updateBookingValidator = updateBookingValidator;
            _queryBookingValidator = queryBookingValidator;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryBookingDto query)
        {
            var validateResult = await _queryBookingValidator.ValidateAsync(query);

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
            
            var response = await _bookingService.GetAll(query);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings([FromQuery] QueryBookingDto query)
        {
            var validateResult = await _queryBookingValidator.ValidateAsync(query);

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

            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            query.UserId = userId;

            var response = await _bookingService.GetAll(query);

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto dto)
        {
            var validateResult = await _createBookingValidator.ValidateAsync(dto);

            if(!validateResult.IsValid)
            {
                var errors = validateResult.Errors
                    .Select(x => x.ErrorMessage);

                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Ошибка валидации",
                    Errors = errors
                });
            }

            int userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var response = await _bookingService.Create(
                dto,
                userId);

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/get")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _bookingService.GetById(id);

            return Ok(response);
        }

        [Authorize]
        [HttpPut("{id}/update")]
        public async Task<IActionResult> Update(
            int id,
            UpdateBookingDto dto)
        {
            var validateResult = await _updateBookingValidator.ValidateAsync(dto);

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

            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var isAdmin = User.IsInRole("Admin");

            var response = await _bookingService.Update(
                id,
                dto,
                userId,
                isAdmin);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{id}/delete")]
        public async Task<IActionResult> Remove(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var isAdmin = User.IsInRole("Admin");

            await _bookingService.Remove(
                id,
                userId,
                isAdmin);

            return NoContent();
        }

        [Authorize]
        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var isAdmin = User.IsInRole("Admin");
                        
            var response = await _bookingService.Confirm(
                id,
                userId,
                isAdmin);

            return Ok(response);
        }

        [Authorize]
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var isAdmin = User.IsInRole("Admin");

            var response = await _bookingService.Cancel(
                id,
                userId,
                isAdmin);

            return Ok(response);
        }
    }
}
