using System.ComponentModel.DataAnnotations;

namespace Tapir.Identity.Infrastructure.Models
{
    public class RefreshToken<TKey> where TKey : IEquatable<TKey>
    {
        [Key]
        public int Id { get; set; }
        public TKey UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Value { get; set; }
    }
}
