namespace FavouriteMons.Models
{
  public class TeamDisplay
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Monsters> Monsters { get; set; }

    public TeamDisplay(Guid id, Guid userId, DateTime createdAt)
    {
      Id = id;
      UserId = userId;
      CreatedAt = createdAt;
      Monsters = new();
    }
  }
}
