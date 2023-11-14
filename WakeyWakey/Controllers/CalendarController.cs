using System;
using Microsoft.AspNetCore.Authorization;
using WakeyWakey.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WakeyWakey.Services;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace WakeyWakey.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ApiService<Event> _eventService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ApiService<Event> eventService, ILogger<CalendarController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        public IActionResult Create()
        {
            return View(new Event());
        }

         public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event calendarEvent) // Changed 'event' to 'calendarEvent'
        {
            if (!ModelState.IsValid)
            {
                return View(calendarEvent);
            }

            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                calendarEvent.UserId = userId;
                await _eventService.AddAsync(calendarEvent);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                return View(calendarEvent);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetEvents()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var events = await _eventService.GetAllAsync();
            var userEvents = events.Where(e => e.UserId == userId).Select(e => new
            {
                id = e.Id,
                title = e.Name,
                start = e.StartDate.ToString("s"), // ISO8601 format
                end = e.EndDate.ToString("s"), // ISO8601 format
                description = e.Description,
                location = e.Location,
                allDay = e.StartDate.TimeOfDay == TimeSpan.Zero && e.EndDate.TimeOfDay == TimeSpan.Zero
            }).ToList();

            return Json(userEvents);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _eventService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            _logger.LogInformation("Create method hit.2");
            

            var calendarEvent = await _eventService.GetByIdAsync(id);
            return View(calendarEvent);
        }




       
    }
}
