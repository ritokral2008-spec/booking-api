namespace BookingApi.DTOs
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = null!;
        public IEnumerable<string> Errors { get; set; } = [];
    }
}
