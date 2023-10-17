using System;

namespace WakeyWakey.Models;

public class ReminderModel
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int SoundId { get; set; } // should it be byte array or an id referenced to a sound table?
    public DateTime ReminderDateTime { get; set; }
}