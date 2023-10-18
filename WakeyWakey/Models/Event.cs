using System;

namespace WakeyWakey.Models
{
    public class Event
    {
        public int Id { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public int? Recurring { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public required int UserId { get; set; }
    }
}

