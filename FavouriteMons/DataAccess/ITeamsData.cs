using FavouriteMons.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace FavouriteMons.DataAccess
{
  [Headers("Authorization: 52cc004d-b5c8-448f-be6b-6b9bd532fe20")]
  public interface ITeamsData
  {
    [Get("/Teams/{id}")]
    Task<List<TeamDisplay>> GetTeamsById(Guid id);

    [Post("/Teams")]
    Task<IActionResult> CreateTeam([Body] TeamNew teamNew);
  }
}
