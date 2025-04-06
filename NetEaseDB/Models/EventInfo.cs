namespace NetEaseDB.Models
{
    public class EventInfo
    {
        //EventInfo table data
        public int EventInfoId { get; set; }

        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public int VenueID { get; set; }

        public List<Booking> Booking { get; set; } = new();
    }
}
