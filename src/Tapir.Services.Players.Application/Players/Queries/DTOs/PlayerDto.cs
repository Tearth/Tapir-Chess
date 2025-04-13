namespace Tapir.Services.Players.Application.Players.Queries.DTOs
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Username { get; set; }
        public string? Country { get; set; }
        public string? AboutMe { get; set; }
    }
}
