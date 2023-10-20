using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WakeyWakey.Models;
using WakeyWakey.Services;
using Ical.Net;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WakeyWakey.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        ApiService<Event> _apiService;

        public DashboardController(ApiService<Event> apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Settings()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile icsFile)
        {
            if (icsFile != null && icsFile.Length > 0)
            {
                // Read the file content.
                using var reader = new StreamReader(icsFile.OpenReadStream());
                var fileContent = await reader.ReadToEndAsync();

                // Parse the .ics file content.
                var calendar = Ical.Net.Calendar.Load(fileContent);
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Ensure the user ID is available.
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID not available.");
                }

                foreach (var evt in calendar.Events)
                {
                    var eventModel = new Event
                    {
                        UserId = int.Parse(userId), // Convert the user ID to integer (or the appropriate type).
                        Name = evt.Summary,
                        StartDate = evt.Start.Value,
                        EndDate = evt.End.Value,
                        Description = evt.Description,
                        Location = evt.Location
                    };
                    await _apiService.AddAsync(eventModel);
                }
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return BadRequest("Invalid file.");
            }
        }



    }
}
