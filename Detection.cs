using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnetbackend
{
    public class Detection
    {
        public string Category { get; set; }

        public string ImageName {  get; set; } 

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }

        [Key]
        public string timeStamp { get; set; }

    }
}
