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
        private readonly IEventApiService _eventsService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(IEventApiService eventService, ILogger<CalendarController> logger)
        {
            _eventsService = eventService;
            _logger = logger;
        }

        // public IActionResult Create()
        // {
        //     return View();
        // }

        public async Task<IActionResult> Index()
        {
            try
            {   
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var events = await _eventsService.GetEventsByUserIdAsync(userId);
                var userEvents = events.ToList();
                
                return View(userEvents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching events in Index method");
                // Handle the error appropriately
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(Event calendarEvent) // Changed 'event' to 'calendarEvent'
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                calendarEvent.UserId = userId;
                await _eventsService.AddAsync(calendarEvent);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetEvents()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var events = await _eventsService.GetAllAsync();
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

        //[HttpPost]                                        OLD DELETE IMPLEMENTATION
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        await _eventsService.DeleteAsync(id);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error deleting event");
        //        return RedirectToAction(nameof(Index));
        //    }
        //} 

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _eventsService.DeleteAsync(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting event");
                return Json(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            _logger.LogInformation("Create method hit.2");
            

            var calendarEvent = await _eventsService.GetByIdAsync(id);
            return RedirectToAction("Index");
        }




       
    }
}