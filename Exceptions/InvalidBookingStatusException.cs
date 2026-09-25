namespace BookingApi.Exceptions
{
    public class InvalidBookingStatusException: Exception
    {
        public InvalidBookingStatusException(string message)
            : base(message)
        {

        }
    }
}
