using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WakeyWakey.Models;
using System.Collections.Generic;

namespace WakeyWakey.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // Handle login logic here
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Register(string username, string password, string email)
    {
        // Handle registration logic here
        return RedirectToAction("Index", "Home");
    }

    
    /*private List<Course> courses = new List<Course>
    {
        new Course { Id = 1, Name = "Course 1" },
        new Course { Id = 2, Name = "Course 2" }
    };

    public IActionResult CreateCourse(string courseName)
    {
        var newCourse = new Course
        {
            Id = courses.Count + 1,
            Name = courseName
        };

        courses.Add(newCourse);

        return RedirectToAction("Course");
    }*/

}

