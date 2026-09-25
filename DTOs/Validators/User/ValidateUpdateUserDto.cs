using BookingApi.DTOs.User;
using FluentValidation;

namespace BookingApi.DTOs.Validators.User
{
    public class ValidateUpdateUserDto
        :AbstractValidator<UpdateUserDto>
    {
        public ValidateUpdateUserDto()
        {
            RuleFor(x => x.Username)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Имя пользователя не может быть пустым или состоять исключительно из пробелов")
                .MinimumLength(3)
                .WithMessage("Имя пользователя не может быть меньше 3 символов")
                .MaximumLength(64)
                .WithMessage("Имя пользователя не может превышать 64 символа");

            RuleFor(x => x.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Email не может быть пустым или состоять исключительно из пробелов")
                .EmailAddress()
                .WithMessage("Email адрес должен быть формата email");

            RuleFor(x => x.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage("Пароль не может быть пустым или состоять исключительно из пробелов")
                .MinimumLength(8)
                .WithMessage("Пароль не может быть меньше 8 символов")
                .MaximumLength(100)
                .WithMessage("Пароль не может превышать 100 символов");
        }
    }
}
