namespace BookingApi.DTOs.Room
{
    public class QueryRoomDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? PricePerNight { get; set; }
        public string SortBy { get; set; } = "id";
        public bool SortDescending { get; set; } = false;
    }
}
