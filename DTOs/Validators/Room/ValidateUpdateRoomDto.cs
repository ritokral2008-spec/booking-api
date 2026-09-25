using BookingApi.DTOs.Room;
using FluentValidation;

namespace BookingApi.DTOs.Validators.Room
{
    public class ValidateUpdateRoomDto:
        AbstractValidator<UpdateRoomDto>
    {
        public ValidateUpdateRoomDto()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Название комнаты не может быть пустым или состоять исключительно из пробелов")
                .MaximumLength(100)
                .WithMessage("Длина названия комнаты не может превышать 100 символов");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Длина описания не может превышать 1000 символов");

            RuleFor(x => x.Address)
                .NotEmpty()
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Строка адреса не может быть пустой или состоять исключительно из пробелов")
                .MaximumLength(300)
                .WithMessage("Длина строки адреса не может превышать 300 символов");

            RuleFor(x => x.PricePerNight)
                .GreaterThan(0)
                .WithMessage("Цена за ночь должна быть больше 0");
        }
    }
}
