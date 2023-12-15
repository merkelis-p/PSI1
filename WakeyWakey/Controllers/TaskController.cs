using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using WakeyWakey.Enums;
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

            _logger.LogInformation($"Courses: {JsonConvert.SerializeObject(courses)}");

            var hierarchySelectList = new List<SelectListItem>();
            foreach (var course in courses)
            {
                hierarchySelectList.Add(new SelectListItem 
                { 
                    Text = $"Course: {course.Name}", 
                    Value = $"Course-{course.Id}", 
                    Disabled = true
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

            var taskModel = new Models.Task 
            {
                UserId = userId,
                Status = Enums.TaskStatus.Incompleted, 
                Category = TaskCategory.Assignment
            };

            return View(taskModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Models.Task task, string subjectOrTaskId)
        {
            task.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError(error.ErrorMessage);
                    }
                }
                return View(task);
            }

            // Parse subjectOrTaskId and set either SubjectId or ParentId
            if (subjectOrTaskId.StartsWith("Subject-"))
            {
                task.SubjectId = int.Parse(subjectOrTaskId.Split('-')[1]);
            }
            else if (subjectOrTaskId.StartsWith("Task-"))
            {
                task.ParentId = int.Parse(subjectOrTaskId.Split('-')[1]);
            }

            if (!task.IsValidAssignment())
            {
                // Handle invalid assignment
                ModelState.AddModelError("", "Invalid subject or task assignment.");
                return View(task);
            }

            await _taskService.AddAsync(task);
            return RedirectToAction(nameof(Index));
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
