using System;
using System.ComponentModel.DataAnnotations;


namespace WakeyWakey.Models;
public class SubjectModel
{
    public required int Id { get; set; }
    public required int CourseId { get; set; }
    public required String Name { get; set; }
    public String? Description { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
}