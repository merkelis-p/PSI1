using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WakeyWakey.Services;
using WakeyWakey.Models;


namespace WakeyWakey.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ITaskApiService _taskService;
        private readonly ICourseApiService _courseService;

        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskApiService taskService, ICourseApiService courseService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _courseService = courseService;
            _logger = logger;

        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var tasks = await _taskService.GetTasksWithHierarchyByUserIdAsync(userId);
            return View(tasks);
        }

        public async Task<IActionResult> Create()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var courses = await _courseService.GetAllHierarchyAsync(userId);
            // print courses to the logger
            _logger.LogInformation($"Courses: {JsonConvert.SerializeObject(courses)}");

            var hierarchySelectList = new List<SelectListItem>();
            foreach (var course in courses)
            {
                // Use optgroup to group subjects under courses
                hierarchySelectList.Add(new SelectListItem
                {
                    Text = $"Course: {course.Name}",
                    Value = $"Course-{course.Id}",
                    Disabled = true // Disable selection of courses
                });
                foreach (var subject in course.Subjects)
                {
                    hierarchySelectList.Add(new SelectListItem
                    {
                        Text = $"Subject: {subject.Name}",
                        Value = $"Subject-{subject.Id}"
                    });

                    foreach (var task in subject.Tasks)
                    {
                        hierarchySelectList.Add(new SelectListItem
                        {
                            Text = $"-- Task: {task.Name}",
                            Value = $"Task-{task.Id}"
                        });
                    }
                }
            }

            ViewBag.HierarchySelectList = hierarchySelectList;

            var taskModel = new Models.Task()
            {
                UserId = userId,
                Status = 0,
                Category = 0
            };

            return View(taskModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Models.Task task)
        {

            task.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Set the UserId for the task

            _logger.LogInformation("Create method called.");

            //Console.WriteLine($"task.SubjectOrTaskId: {task.SubjectOrTaskId}");

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError(error.ErrorMessage);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(task.SubjectOrTaskId))
                {
                    string[] parts = task.SubjectOrTaskId.Split('-');

                    if (parts.Length >= 2 && int.TryParse(parts[1], out int subjectId))
                    {
                        task.SubjectId = subjectId;
                    }
                    else
                    {
                        _logger.LogError("Invalid SubjectOrTaskId format: " + task.SubjectOrTaskId);
                    }
                    await _taskService.AddAsync(task);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            if (task.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Unauthorized(); // Prevent editing of tasks not owned by the user
            }

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            if (task.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Unauthorized(); // Prevent editing of tasks not owned by the user
            }

            if (ModelState.IsValid)
            {
                await _taskService.UpdateAsync(id, task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            if (task.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Unauthorized(); // Prevent deletion of tasks not owned by the user
            }

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task.UserId != int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return Unauthorized(); // Prevent deletion of tasks not owned by the user
            }

            await _taskService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }




    }
}
