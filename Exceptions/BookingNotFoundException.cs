namespace BookingApi.Exceptions
{
    public class BookingNotFoundException : Exception
    {
        public BookingNotFoundException(string message)
            : base(message)
        {

        }
    }
}
