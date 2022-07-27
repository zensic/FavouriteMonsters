using System.ComponentModel.DataAnnotations;

namespace FavouriteMons.Models
{
    public class UploadResult
    {
        [Key]
        public int Id { get; set; }
        public string UploadResultAsJson { get; set; }
    }
}
