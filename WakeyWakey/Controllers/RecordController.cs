using WakeyWakey.Services;
using WakeyWakey.Models;
using Microsoft.AspNetCore.Mvc;
using WakeyWakey.Exceptions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WakeyWakey.Controllers;
[Authorize]
public class RecordController : Controller
{
    private readonly ApiService<Record> _timeTracker;
    private readonly ILogger<RecordController> _logger;

    public RecordController(ApiService<Record> timeTracker, ILogger<RecordController> logger)
    {
        _timeTracker = timeTracker;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var times = await _timeTracker.GetAllAsync();
        return View(times);
    }

    public async Task<IActionResult> TimerView(int id)
    {
        var Record = await _timeTracker.GetByIdAsync(id);

        if (Record == null)
        {
            return NotFound();
        }

        ViewBag.FocusDuration = Record.FocusDuration;
        ViewBag.BreakDuration = Record.BreakDuration;

        return View(Record);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Record Record, int focusHours,
                                        int focusMinutes, int breakHours, int breakMinutes,
                                        int breakFrequency)
    {
        int focusDurationSeconds = (focusHours * 60 + focusMinutes) * 60;
        int breakDurationSeconds = (breakHours * 60 + breakMinutes) * 60;
        int TotalDurationSeconds = focusDurationSeconds + breakDurationSeconds;

        Record.Duration = TimeSpan.FromSeconds(TotalDurationSeconds);
        Record.FocusDuration = TimeSpan.FromSeconds(focusDurationSeconds);
        Record.BreakDuration = TimeSpan.FromSeconds(breakDurationSeconds);
        Record.BreakFreaquency = breakFrequency;

        try
        {
            if (focusDurationSeconds < 0)
            {
                throw new CustomValidationException("Value must be greater than or equal to 0.");
            }
            else if (ModelState.IsValid)
            {
                await _timeTracker.AddAsync(Record);
                return RedirectToAction("Index");
            }
        }

        catch (CustomValidationException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            _logger.LogError(e, "Custom validation exception occurred");
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, "An error occurred while processing the request. Please try again.");
            _logger.LogError(e, "Error during record creation");
        }


        return View(Record);
    }



    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var Record = await _timeTracker.GetByIdAsync(id);
        if (Record == null)
        {
            return NotFound();
        }
        return View(Record);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Record Record, int id)
    {
        if (id != Record.RecorderId)
        {
            return BadRequest();
        }
        if (ModelState.IsValid)
        {
            await _timeTracker.UpdateAsync(id, Record);
            return RedirectToAction("Index");
        }
        return View(Record);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var Record = await _timeTracker.GetByIdAsync(id);

        if (Record == null)
        {
            return NotFound();
        }

        return View(Record);
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id, Record Record)
    {
        bool result = await _timeTracker.DeleteAsync(id);

        if (result)
        {
            return RedirectToAction("Index");
        }

        return NotFound(Record);
    }

    [HttpPost]
    public IActionResult StopCountdown([FromBody] Record model)
    {
        model.EndTimeStamp = DateTime.Now;
        model.Duration = model.EndTimeStamp - model.StartTimeStamp;

        return Json(new { success = true });
    }

    [HttpPost]
    public async Task<IActionResult> SaveState(TimeSpan Duration)
    {
        // add the Duration to the total time of Task or Subject or Record
        return Json(Ok());
    }

    public IActionResult Assigment() //
    {
        return View();
    }

    //[HttpPost]
    //public async Task<IActionResult> Start(int id)
    //{
    //    var result = await _RecordService.StartAsync(id);

    //    if (result != null)
    //    {
    //        return PartialView("_TimerPartial", result);
    //    }
    //    return NotFound();
    //}

    //[HttpGet]
    public IActionResult Start(int id)
    {
        var startTime = DateTime.Now;

        // Simulate starting the timer in a separate thread
        Task.Run(() =>
        {
            // _RecordService.StartTimer(id, startTime);
        });

        return RedirectToAction("Index");
    }

}
