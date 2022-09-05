using System.ComponentModel;
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
    [DisplayName("Element")]
    public Guid ElementId { get; set; }
    public string ImageUrl { get; set; }
    [Range(1, 255, ErrorMessage = "Value must be between 1 and 255")]
    public int HP { get; set; }
    [Range(1, 255, ErrorMessage = "Value must be between 1 and 255")]
    public int Attack { get; set; }
    [Range(1, 255, ErrorMessage = "Value must be between 1 and 255")]
    public int Defence { get; set; }
    [Range(1, 255, ErrorMessage = "Value must be between 1 and 255")]
    public int Speed { get; set; }
  }
}
