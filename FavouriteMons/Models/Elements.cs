using System.ComponentModel.DataAnnotations;

namespace FavouriteMons.Models
{
  public class Elements
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }
    public string Color { get; set; }
  }
}
