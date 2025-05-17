namespace Tapir.Services.Games.Application.Rooms.Queries.DTOs
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public int Time { get; set; }
        public int Increment { get; set; }
    }
}
