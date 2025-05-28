using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace NetEaseDB.Models
{
    public class EventInfo
    {
        //EventInfo table data
        public int EventInfoId { get; set; }

        public string? EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string? Description { get; set; }

        public int VenueID { get; set; }

        public Venue? Venue { get; set; }

        public int EventTypeID { get; set; }//step 4 foreign key column
        public EventType? EventType { get; set; } //nav property to EventType object 


        public List<Booking> Booking { get; set; } = new();
    }
}
