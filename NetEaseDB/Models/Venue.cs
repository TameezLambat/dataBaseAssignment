using System.ComponentModel.DataAnnotations.Schema;

namespace NetEaseDB.Models
{
    public class Venue
    {
        //Venue table data in the database
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string? ImageURL { get; set; } //IFormFile?
        public List<Booking> Booking { get; set; } = new();
        // Used only for uploading (not stored in DB)
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
