using System;
using WakeyWakey.Models;
using Microsoft.AspNetCore.Mvc;

namespace WakeyWakey.Controllers
{
	public class CourseController : Controller
	{
        public IActionResult Index()
        {
            List<Course> courselist = new List<Course>();
            Course course = new Course();
            course.ID = 1;
            course.Name = "PSI";
            courselist.Add(course);
            Course course2 = new Course();
            course2.ID = 2;
            course2.Name = "Komparch";
            courselist.Add(course2);
            ViewData["StudentID"] = 21;
            ViewBag.Description = "Very difficult";
            string uni2 = "belekoks";
            TempData["uni"] = uni2;
            return View(courselist);
        }

        public JsonResult GetDateWithJson()
        {
            string JsonDate = DateTime.Today.ToShortDateString();
            return Json(JsonDate);
        }

        public IActionResult AddCourse()
        {
            Course course = new Course();
            return View(course);
        }


        [HttpPost]
        public ActionResult AddCourse(Course course)
        {
            string textvalue = "";
            if (ModelState.IsValid)
                textvalue = "Model state is valid";
            else
                textvalue = "Model state is not valid";
            return View(course);
        }

        public IActionResult UpdateCourse()
        {
            Course course = new Course();
            course.ID = 1;
            string uni = (string)TempData["uni"];
            course.Name = uni;
            return View(course);
        }

        [HttpPost]
        public IActionResult UpdateCourse(Course course)
        {
            return View(course);
        }
    }
}

