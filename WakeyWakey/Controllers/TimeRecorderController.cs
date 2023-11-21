using WakeyWakey.Services;
using WakeyWakey.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WakeyWakey.Controllers;
[Authorize] 
public class TimeRecorderController : Controller
{
    private readonly ApiService<TimeRecorder> _timeTracker;

    public TimeRecorderController(ApiService<TimeRecorder> timeTracker)
    {
        _timeTracker = timeTracker;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _timeTracker.GetAllAsync());
    }

    public async Task<IActionResult> TimerView(int id)
    {
        var timeRecorder = await _timeTracker.GetByIdAsync(id);

        if (timeRecorder == null)
        {
            return NotFound();
        }

        ViewBag.FocusDuration = timeRecorder.FocusDuration;
        ViewBag.BreakDuration = timeRecorder.BreakDuration;

        return View(timeRecorder);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TimeRecorder timeRecorder,int focusHours,
                                            int focusMinutes, int breakHours,int breakMinutes,
                                            int breakFrequency)
    {
        int focusDurationSeconds = (focusHours * 60 + focusMinutes) * 60;
        int breakDurationSeconds = (breakHours * 60 + breakMinutes) * 60;
        int TotalDurationSeconds = focusDurationSeconds + breakDurationSeconds;

        timeRecorder.Duration = TimeSpan.FromSeconds(TotalDurationSeconds);
        timeRecorder.FocusDuration = TimeSpan.FromSeconds(focusDurationSeconds);
        timeRecorder.BreakDuration = TimeSpan.FromSeconds(breakDurationSeconds);
        timeRecorder.BreakFreaquency = breakFrequency;

        if (ModelState.IsValid)
        {
            await _timeTracker.AddAsync(timeRecorder);
            return RedirectToAction("Index");
        }

        return View(timeRecorder);
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var timeRecorder = await _timeTracker.GetByIdAsync(id);
        if(timeRecorder == null)
        {
            return NotFound();
        }
        return View(timeRecorder);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TimeRecorder timeRecorder, int id)
    {
        if(id != timeRecorder.RecorderId)
        {
            return BadRequest();
        }
        if (ModelState.IsValid)
        {
            await _timeTracker.UpdateAsync(id, timeRecorder);
            return RedirectToAction("Index");
        }
        return View(timeRecorder);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var timeRecorder = await _timeTracker.GetByIdAsync(id);

        if (timeRecorder == null)
        {
            return NotFound();
        }

        return View(timeRecorder);
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id, TimeRecorder timeRecorder)
    {
        bool result = await _timeTracker.DeleteAsync(id);

        if (result)
        {
            return RedirectToAction("Index");
        }

        return NotFound(timeRecorder);
    }

    public IActionResult Assigment() //
    {
        return View();
    }

    //[HttpPost]
    //public async Task<IActionResult> Start(int id)
    //{
    //    // Assume the id parameter represents the TimeRecorder entity to start
    //    var result = await _timeRecorderService.StartAsync(id);

    //    if (result != null)
    //    {
    //        // Return a partial view with the updated timer information
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
           // _timeRecorderService.StartTimer(id, startTime);
        });

        return RedirectToAction("Index");
    }

    public IActionResult Stop()///
    {
        return View();
    }
    public IActionResult Pause()///
    {
        return View();
    }

}
