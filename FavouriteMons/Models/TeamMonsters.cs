using System.ComponentModel.DataAnnotations;

namespace FavouriteMons.Models
{
    public class TeamMonsters
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid TeamId { get; set; }
        [Required]
        public Guid MonsterId { get; set; }
    }
}
