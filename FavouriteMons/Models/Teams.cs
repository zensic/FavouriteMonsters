using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavouriteMons.Models
{
    public class Teams
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("Users")]
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
