using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WakeyWakey.Models;
using WakeyWakey.Services;
using Newtonsoft.Json;

namespace WakeyWakey.Controllers
{
    [Authorize]
    [Route("Course/{courseId}/[controller]")]
    public class SubjectController : Controller
    {
        private readonly SubjectApiService _subjectService;
        private readonly ILogger<SubjectController> _logger;

        public SubjectController(SubjectApiService subjectService, ILogger<SubjectController> logger)
        {
            _subjectService = subjectService;
            _logger = logger;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index(int courseId)
        {
            var subjects = await _subjectService.GetSubjectsByCourseIdAsync(courseId);
            if (subjects == null || !subjects.Any())
            {
                ViewBag.IsEmpty = true;
                ViewBag.CourseId = courseId;
                return View(new List<Subject>()); 
            }

            return View(subjects);
        }

        [HttpGet("Create")]
        public IActionResult Create(int courseId)
        {
            ViewBag.CourseId = courseId;
            return View(new Subject { CourseId = courseId });
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(int courseId, Subject subject)
        {
            _logger.LogInformation($"Invalid! Trying to add subject: {JsonConvert.SerializeObject(subject)} for course ID: {courseId}");
            
            if (ModelState.IsValid)
            {
                subject.CourseId = courseId;  // Ensure the subject is linked to the course
                _logger.LogInformation($"Trying to add subject: {JsonConvert.SerializeObject(subject)} for course ID: {courseId}");

                await _subjectService.AddAsync(subject);
                return RedirectToAction(nameof(Index), new { courseId });
            }
            return View(subject);
        }

        
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int courseId, int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int courseId, int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _subjectService.UpdateAsync(id, subject);
                return RedirectToAction(nameof(Index), new { courseId });
            }
            return View(subject);
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int courseId, int id)
        {
            var subject = await _subjectService.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int courseId, int id)
        {
            await _subjectService.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { courseId });
        }
    }
}
