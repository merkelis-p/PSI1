using Microsoft.AspNetCore.Mvc;
using WakeyWakey.Models;
using WakeyWakey.Services;

public class SubjectController : Controller
{
    private readonly SubjectRepository _subjectRepository;

    public SubjectController(SubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public IActionResult Index()
    {
        var subjects = _subjectRepository.GetAllSubjects(); // extention method fulfilled
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
            _subjectRepository.AddSubject(subject);
            return RedirectToAction("Index");
        }
        return View(subject);
    }

    public IActionResult Edit(int id)
    {
        var subject = _subjectRepository.GetSubject(id);
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
            _subjectRepository.UpdateSubject(subject);
            return RedirectToAction("Index");
        }
        return View(subject);
    }

    public IActionResult Delete(int id)
    {
        var subject = _subjectRepository.GetSubject(id);
        if (subject == null)
        {
            return NotFound();
        }
        return View(subject);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        _subjectRepository.DeleteSubject(id);
        return RedirectToAction("Index");
    }
}
