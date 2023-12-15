using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WakeyWakey.Controllers;
using WakeyWakey.Models;
using WakeyWakey.Services;
using Task = System.Threading.Tasks.Task;

namespace WakeyWakey.Tests.ControllersTests
{
    public class CourseControllerTests
    {
        private readonly Mock<ICourseApiService> _mockCourseService;
        private readonly CourseController _controller;
        private readonly ClaimsPrincipal _user;

        public CourseControllerTests()
        {
            // Mock the Course API Service
            _mockCourseService = new Mock<ICourseApiService>();
            // Mock the User context
            _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));

            // Instantiate the controller with the mocked services
            _controller = new CourseController(_mockCourseService.Object, Mock.Of<ILogger<CourseController>>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = _user } // Set the User on the HttpContext
                }
            };
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithCourses()
        {
            // Arrange
            var expectedCourses = new List<Course>
            {
                new Course { Id = 1, UserId = 1, Name = "Test Course 1" },
                new Course { Id = 2, UserId = 1, Name = "Test Course 2" }
            };
            _mockCourseService.Setup(service => service.GetAllAsync())
                .ReturnsAsync(expectedCourses);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Course>>(viewResult.Model);
            var courseList = Assert.IsType<List<Course>>(model);
            Assert.Equal(expectedCourses.Count, courseList.Count);
            for (int i = 0; i < expectedCourses.Count; i++)
            {
                Assert.Equal(expectedCourses[i].Id, courseList[i].Id);
                Assert.Equal(expectedCourses[i].Name, courseList[i].Name);
                Assert.Equal(expectedCourses[i].UserId, courseList[i].UserId);
            }
        }
        
                [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<Course>(viewResult.Model);
        }

        [Fact]
        public async Task Create_PostValidCourse_ReturnsRedirectToActionResult()
        {
            // Arrange
            var newCourse = new Course { Name = "New Course" };
            _mockCourseService.Setup(s => s.AddAsync(It.IsAny<Course>())).ReturnsAsync(newCourse);

            // Act
            var result = await _controller.Create(newCourse);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Edit_GetValidId_ReturnsViewResultWithCourse()
        {
            // Arrange
            var courseToEdit = new Course { Id = 1, Name = "Existing Course" };
            _mockCourseService.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(courseToEdit);

            // Act
            var result = await _controller.Edit(courseToEdit.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Course>(viewResult.Model);
            Assert.Equal(courseToEdit.Id, model.Id);
            Assert.Equal(courseToEdit.Name, model.Name);
        }

        [Fact]
        public async Task Edit_PostValidCourse_ReturnsRedirectToActionResult()
        {
            // Arrange
            var courseToEdit = new Course { Id = 1, Name = "Updated Course" };
            _mockCourseService.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<Course>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Edit(courseToEdit.Id, courseToEdit);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        
        [Fact]
        public async Task Edit_PostInvalidModelState_ReturnsViewResultWithCourse()
        {
            // Arrange
            var courseId = 1;
            var courseToEdit = new Course { Id = courseId, Name = "Invalid Course" };
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Edit(courseId, courseToEdit);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.Equal(courseToEdit, viewResult.Model);
        }

        [Fact]
        public async Task Delete_GetValidId_ReturnsViewResultWithCourse()
        {
            // Arrange
            var courseId = 1;
            var courseToDelete = new Course { Id = courseId, Name = "Course To Delete" };
            _mockCourseService.Setup(s => s.GetByIdAsync(courseId)).ReturnsAsync(courseToDelete);

            // Act
            var result = await _controller.Delete(courseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Course>(viewResult.Model);
            Assert.Equal(courseToDelete.Id, model.Id);
        }

        [Fact]
        public async Task DeleteConfirmed_PostValidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var courseId = 1;
            _mockCourseService.Setup(s => s.DeleteAsync(courseId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteConfirmed(courseId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        
        
        [Fact]
        public async Task Edit_GetInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidCourseId = 999; // Assume this ID does not exist
            _mockCourseService.Setup(s => s.GetByIdAsync(invalidCourseId)).ReturnsAsync((Course)null);

            // Act
            var result = await _controller.Edit(invalidCourseId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_PostInvalidId_ReturnsRedirectToActionResult()
        {
            // Arrange
            var invalidCourseId = 999; // Assume this ID does not exist
            _mockCourseService.Setup(s => s.DeleteAsync(invalidCourseId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteConfirmed(invalidCourseId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }


        [Fact]
        public async Task Create_PostInvalidModelState_ReturnsViewWithCourse()
        {
            // Arrange
            var course = new Course { Name = "Invalid Course" };
            _controller.ModelState.AddModelError("Error", "Some error message");

            // Act
            var result = await _controller.Create(course);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(course, viewResult.Model);
            Assert.False(_controller.ModelState.IsValid);
        }
        
    }
}
