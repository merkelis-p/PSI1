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
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace WakeyWakey.Tests.ControllersTests
{
    public class SubjectControllerTests
    {
        private readonly Mock<ISubjectApiService> _mockSubjectService;
        private readonly SubjectController _controller;
        private readonly ClaimsPrincipal _user;

        public SubjectControllerTests()
        {
            // Mock the Subject API Service
            _mockSubjectService = new Mock<ISubjectApiService>();
            // Mock the User context
            _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
            }, "mock"));

            // Instantiate the controller with the mocked services
            _controller = new SubjectController(_mockSubjectService.Object, Mock.Of<ILogger<SubjectController>>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = _user } // Set the User on the HttpContext
                }
            };
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithSubjects()
        {
            // Arrange
            int courseId = 1;
            var expectedSubjects = new List<Subject>
            {
                new Subject { Id = 1, CourseId = courseId, Name = "Test Subject 1" },
                new Subject { Id = 2, CourseId = courseId, Name = "Test Subject 2" }
            };
            _mockSubjectService.Setup(service => service.GetSubjectsByCourseIdAsync(courseId))
                .ReturnsAsync(expectedSubjects);

            // Act
            var result = await _controller.Index(courseId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Subject>>(viewResult.Model);
            var subjectList = Assert.IsType<List<Subject>>(model);
            Assert.Equal(expectedSubjects.Count, subjectList.Count);
            for (int i = 0; i < expectedSubjects.Count; i++)
            {
                Assert.Equal(expectedSubjects[i].Id, subjectList[i].Id);
                Assert.Equal(expectedSubjects[i].Name, subjectList[i].Name);
                Assert.Equal(expectedSubjects[i].CourseId, subjectList[i].CourseId);
            }
        }

// ...

    // Test for Create GET Action
    [Fact]
    public void Create_Get_ReturnsViewResult()
    {
        int courseId = 1;

        // Act
        var result = _controller.Create(courseId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Subject>(viewResult.Model);
        Assert.Equal(courseId, model.CourseId);
    }

    // Test for Create POST Action
    [Fact]
    public async Task Create_PostValidSubject_ReturnsRedirectToActionResult()
    {
        int courseId = 1;
        var newSubject = new Subject { Name = "New Subject" };

        _mockSubjectService.Setup(s => s.AddAsync(It.IsAny<Subject>()))
            .ReturnsAsync(newSubject);

        // Act
        var result = await _controller.Create(courseId, newSubject);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal(courseId, redirectToActionResult.RouteValues["courseId"]);
    }

    // Test for Edit GET Action
    [Fact]
    public async Task Edit_GetValidId_ReturnsViewResultWithSubject()
    {
        int courseId = 1;
        int subjectId = 1;
        var subjectToEdit = new Subject { Id = subjectId, Name = "Existing Subject", CourseId = courseId };

        _mockSubjectService.Setup(s => s.GetByIdAsync(subjectId))
            .ReturnsAsync(subjectToEdit);

        // Act
        var result = await _controller.Edit(courseId, subjectId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Subject>(viewResult.Model);
        Assert.Equal(subjectToEdit.Id, model.Id);
        Assert.Equal(subjectToEdit.Name, model.Name);
    }

    // Test for Edit POST Action
    [Fact]
    public async Task Edit_PostValidSubject_ReturnsRedirectToActionResult()
    {
        int courseId = 1;
        var subjectToEdit = new Subject { Id = 1, Name = "Updated Subject", CourseId = courseId };

        _mockSubjectService.Setup(s => s.UpdateAsync(subjectToEdit.Id, subjectToEdit))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Edit(courseId, subjectToEdit.Id, subjectToEdit);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal(courseId, redirectToActionResult.RouteValues["courseId"]);
    }

    // Test for Delete GET Action
    [Fact]
    public async Task Delete_GetValidId_ReturnsViewResultWithSubject()
    {
        int courseId = 1;
        int subjectId = 1;
        var subjectToDelete = new Subject { Id = subjectId, Name = "Subject To Delete", CourseId = courseId };

        _mockSubjectService.Setup(s => s.GetByIdAsync(subjectId))
            .ReturnsAsync(subjectToDelete);

        // Act
        var result = await _controller.Delete(courseId, subjectId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Subject>(viewResult.Model);
        Assert.Equal(subjectToDelete.Id, model.Id);
    }

    // Test for Delete POST Action
    [Fact]
    public async Task DeleteConfirmed_PostValidId_ReturnsRedirectToActionResult()
    {
        int courseId = 1;
        int subjectId = 1;

        _mockSubjectService.Setup(s => s.DeleteAsync(subjectId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteConfirmed(courseId, subjectId);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal(courseId, redirectToActionResult.RouteValues["courseId"]);
    }

// Add the tests for invalid cases (invalid model state, invalid IDs, etc.)

// ...
        // Follow the same pattern as the Index test above, mocking the necessary service calls and asserting the expected results

        // Remember to test different scenarios such as valid/invalid model states, valid/invalid IDs, and so on.
    }
}
