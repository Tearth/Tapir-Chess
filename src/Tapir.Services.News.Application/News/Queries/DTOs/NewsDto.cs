namespace Tapir.Services.News.Application.News.Queries.DTOs
{
    public class NewsDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Title { get; set; }
        public string? Alias { get; set; }
        public string? Content { get; set; }
    }
}
