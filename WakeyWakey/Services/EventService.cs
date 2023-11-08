using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WakeyWakey.Models;

namespace WakeyWakey.Services;

public class EventService : IEntityService<Event>
{
    private List<Event> _events;
    private readonly string _dataFilePath = "Services/Events.csv";


    public EventService()
    {
        _events = LoadEventsFromCsv();
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<Event> GetByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task<Event> AddAsync(EventModel event)
    {
        var newEvent = new EventModel
        {
            Name = event.Name,
            Description = event.Description,
            StartDateTime = event.StartDateTime,
            EndDateTime = event.EndDateTime,
            CourseId = event.CourseId
        };
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return newEvent;
    }




}