﻿using FavouriteMons.Models;
using Refit;


namespace FavouriteMons.DataAccess
{
  [Headers("Authorization: 52cc004d-b5c8-448f-be6b-6b9bd532fe20")]
  public interface IElementsData
  {
    [Get("/Elements")]
    Task<List<Elements>> GetElements();
  }
}
