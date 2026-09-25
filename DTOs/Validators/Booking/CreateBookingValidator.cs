using BookingApi.DTOs.Booking;
using FluentValidation;

namespace BookingApi.DTOs.Validators.Booking
{
    public class CreateBookingValidator:
        AbstractValidator<CreateBookingDto>
    {
        public CreateBookingValidator()
        {
            RuleFor(x => x.RoomId)
                .GreaterThan(0)
                .WithMessage("RoomId должно быть больше 0");

            RuleFor(x => x.CheckIn)
                .GreaterThan(DateTime.Now)
                .WithMessage("Дата заезда не может быть раньше сегодняшнего дня");

            RuleFor(x => x.CheckOut)
                .GreaterThan(x => x.CheckIn)
                .WithMessage("Дата выезда должна быть позже даты заезда");
        }
    }
}
