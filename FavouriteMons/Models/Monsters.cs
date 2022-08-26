using System.ComponentModel.DataAnnotations;

namespace FavouriteMons.Models
{
  public class Monsters
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; }
    [Required]
    public Guid ElementId { get; set; }
    public string ImageUrl { get; set; }
    public int Votes { get; set; } = 0;
  }
}
