using System;

namespace WakeyWakey.Models;

public class EventModel
{
    public int Id { get; set; } /// ar ID ar EventID?
    public int RecrutingEventNumber { get; set; }
    public String Name { get; set; }
    public String Location { get; set; }
    public String Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
}