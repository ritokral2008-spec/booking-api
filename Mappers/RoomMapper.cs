using BookingApi.DTOs.Room;
using BookingApi.Models;

namespace BookingApi.Mappers
{
    public class RoomMapper
    {
        public static ResponseRoomDto ToDto(Room room)
        {
            return new ResponseRoomDto
            {
                Id = room.Id,
                Title = room.Title,
                Description = room.Description,
                Address = room.Address,
                PricePerNight = room.PricePerNight
            };
        }
    }
}
