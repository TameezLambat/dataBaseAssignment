namespace NetEaseDB.Models
{
    public class Booking
    {
        
            public int BookingID { get; set; }  // Primary Key

            public int EventInfoID { get; set; }  // Foreign Key to Event
            public EventInfo? EventInfo { get; set; }  // Navigation Property

            public int VenueID { get; set; }  // Foreign Key to Venue
            public Venue? Venue { get; set; }  // Navigation Property

            public DateTime BookingDate { get; set; } = DateTime.Now;  // Default value (like GETDATE() in SQL)
        }

    }

