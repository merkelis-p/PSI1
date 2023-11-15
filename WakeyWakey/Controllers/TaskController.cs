using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WakeyWakey.Services;
using WakeyWakey.Models;


namespace WakeyWakey.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ApiService<Models.Task> _taskService;
        private readonly ApiService<Subject> _subjectService;

        private readonly ILogger<TaskController> _logger;

        public TaskController(ApiService<Models.Task> taskService,  ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var tasks = await _taskService.GetAllAsync();
            var userTasks = tasks.Where(task => task.UserId == userId).ToList();
            return View(userTasks);
        }

        public IActionResult Create()
        {
            // get all subjects for the user
            
            
            
            // pass new task to the view
            var task = new Models.Task() 
            {
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), 
                Status = 0, 
                Category = 0
            };

            return View(task); // Pass the task to the view
        }


        
        [HttpPost]
        public async Task<IActionResult> Create(Models.Task task)
        {
            
            task.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // Set the UserId for the task

            _logger.LogInformation("Create method called.");
    
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError(error.ErrorMessage);
                    }
                }
            } else 
            {
                await _taskService.AddAsync(task);
                return RedirectToAction(nameof(Index));
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
