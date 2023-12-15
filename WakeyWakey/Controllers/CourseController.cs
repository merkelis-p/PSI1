using System.Drawing.Printing;
using Microsoft.AspNetCore.Authorization;
using WakeyWakey.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WakeyWakey.Services;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace WakeyWakey.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseApiService _courseService;
        private readonly ILogger<CourseController> _logger;
        private readonly CourseStatusService _courseStatusService;

        public CourseController(ICourseApiService courseService, ILogger<CourseController> logger, CourseStatusService courseStatusService)
        {
            _courseService = courseService;
            _logger = logger;
            _courseStatusService = courseStatusService;
        }
        
        public IActionResult Create()
        {
            return View(new Course()); ///////// delete new Course() - no need
        }
        
        public async Task<IActionResult> Index()
        {
            
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var courses = await _courseService.GetAllAsync();
            var userCourses = courses.Where(course => course.UserId == userId).ToList();

            var courseStatusResults = await _courseStatusService.GetCourseStatus(User);

            ViewBag.courseStatusResults = courseStatusResults;

            return View(userCourses);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            _logger.LogInformation("Create method hit.");
            _logger.LogInformation($"ModelState errors count: {ModelState.ErrorCount}");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogInformation(error.ErrorMessage);
                }
            }

            // Set the UserId for the course before saving it
            course.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (ModelState.IsValid)
            {
                
                await _courseService.AddAsync(course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }


        public async Task<IActionResult> Edit(int id)
        {
            
            _logger.LogInformation("Create method hit.1");
            _logger.LogInformation($"ModelState errors count: {ModelState.ErrorCount}");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogInformation(error.ErrorMessage);
                }
            }
            
            
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            
            _logger.LogInformation("Create method hit.2");
            _logger.LogInformation($"ModelState errors count: {ModelState.ErrorCount}");
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogInformation(error.ErrorMessage);
                }
            }
            
            
            if (id != course.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _courseService.UpdateAsync(id, course);
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _courseService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
