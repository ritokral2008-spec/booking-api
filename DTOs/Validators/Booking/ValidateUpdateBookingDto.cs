using FluentValidation;
using BookingApi.DTOs.Booking;

namespace BookingApi.DTOs.Validators.Booking
{
    public class ValidateUpdateBookingDto:
        AbstractValidator<UpdateBookingDto>
    {
        public ValidateUpdateBookingDto()
        {
            RuleFor(x => x.RoomId)
                .GreaterThan(0)
                .WithMessage("RoomId должен быть больше 0");

            RuleFor(x => x.CheckIn)
                .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage("Дата заезда не может быть раньше сегодняшнего дня");

            RuleFor(x => x.CheckOut)
                .GreaterThan(x => x.CheckIn)
                .WithMessage("Дата выезда должна быть позже даты заезда");
        }
    }
}
