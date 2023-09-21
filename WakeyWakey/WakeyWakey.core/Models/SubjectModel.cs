using System;

namespace WakeyWakey.Models

public class SubjectModel
{
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public int CourseId { get; set; }
    public String Name { get; set; }
    public String Description { get; set; }
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
}