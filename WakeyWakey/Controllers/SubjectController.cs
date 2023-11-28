using Microsoft.AspNetCore.Mvc;
using WakeyWakey.Models;
using WakeyWakey.Services;

namespace WakeyWakey.Controllers;

public class SubjectController : Controller
{
    private readonly SubjectStreamReader _subjectStreamReader;

    public SubjectController(SubjectStreamReader subjectStreamReader)
    {
        _subjectStreamReader = subjectStreamReader;
    }

    public IActionResult Index()
    {
        var subjects = _subjectStreamReader.GetAllSubjects(); // extention method fulfilled
        return View(subjects);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(SubjectModel subject)
    {
        if (ModelState.IsValid)
        {
            _subjectStreamReader.AddSubject(subject);
            return RedirectToAction("Index");
        }
        return View(subject);
    }

    public IActionResult Edit(int id)
    {
        var subject = _subjectStreamReader.GetSubject(id);
        if (subject == null)
        {
            return NotFound();
        }
        return View(subject);
    }

    [HttpPost]
    public IActionResult Edit(SubjectModel subject)
    {
        if (ModelState.IsValid)
        {
            _subjectStreamReader.UpdateSubject(subject);
            return RedirectToAction("Index");
        }
        return View(subject);
    }

    public IActionResult Delete(int id)
    {
        var subject = _subjectStreamReader.GetSubject(id);
        if (subject == null)
        {
            return NotFound();
        }
        return View(subject);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        _subjectStreamReader.DeleteSubject(id);
        return RedirectToAction("Index");
    }
}
