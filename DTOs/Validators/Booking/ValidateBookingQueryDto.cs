using BookingApi.DTOs.Booking;
using FluentValidation;

namespace BookingApi.DTOs.Validators.Booking
{
    public class ValidateBookingQueryDto:
        AbstractValidator<QueryBookingDto>
    {
        public ValidateBookingQueryDto()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Минимальный номер страницы - 1");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Минимальный размер страницы - 1")
                .LessThanOrEqualTo(100)
                .WithMessage("Максимальный размер страницы - 100");

            RuleFor(x => x.SortBy)
                .Must(x =>
                    x == null ||
                    x.Equals("id", StringComparison.OrdinalIgnoreCase) ||
                    x.Equals("checkin", StringComparison.OrdinalIgnoreCase) ||
                    x.Equals("checkout", StringComparison.OrdinalIgnoreCase) ||
                    x.Equals("status", StringComparison.OrdinalIgnoreCase) ||
                    x.Equals("totalprice", StringComparison.OrdinalIgnoreCase))
                .WithMessage(
                    "Сортировка возможна по: id, checkin, checkout, status, totalprice");
        }
    }
}
