using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NetEaseDB.Models
{
    public class EventType
    {
        [Key]
        public int EventTypeID { get; set; }

        [Required]
        public string Name { get; set; }

        // Navigation property - one EventType can be linked to many EventInfo records
        public List<EventInfo> EventInfo { get; set; } = new();
    }
}
