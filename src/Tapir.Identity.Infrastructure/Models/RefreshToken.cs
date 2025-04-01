using System.ComponentModel.DataAnnotations;

namespace Tapir.Identity.Infrastructure.Models
{
    public class RefreshToken<TKey> where TKey : IEquatable<TKey>
    {
        [Key]
        public int Id { get; set; }

        public required TKey UserId { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required string Value { get; set; }
    }
}
