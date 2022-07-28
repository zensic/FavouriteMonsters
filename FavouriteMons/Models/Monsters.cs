using System.ComponentModel.DataAnnotations;

namespace FavouriteMons.Models
{
    public class Monsters
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Element Element { get; set; }
        public string ImageUrl { get; set; }
    }

    public enum Element
    {
        Fire, Grass, Water
    }
}
