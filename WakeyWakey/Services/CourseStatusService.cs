using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WakeyWakey.Services
{
    [Authorize]
    public class CourseStatusService
    {
        private ICourseApiService CourseService;
        public CourseStatusService(ICourseApiService courseService)
        {
            CourseService = courseService;
        }
        public async Task<Dictionary<int, (int ProgressTime, int? Score, int courseProgress)>> GetCourseStatus(ClaimsPrincipal user)
        {
            int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            var courses = await CourseService.GetAllHierarchyAsync(userId);

            var results = new Dictionary<int, (int ProgressTime, int? Score, int courseProgress)>();

            foreach (var course in courses)
            {
                int? score = 0;
                int progress = 0;
                int taskCount = 0;

                foreach (var subject in course.Subjects)
                {
                    foreach (var task in subject.Tasks)
                    {
                        score += task.Score * task.ScoreWeight / 10;
                        taskCount++;

                        if (task.Status == Enums.TaskStatus.Completed)
                        {
                            progress++;
                        }
                    }
                }

                DateTime? StartTime = course.StartDate;
                DateTime? EndTime = course.EndDate;

                TimeSpan totalCourseTime = EndTime.Value - StartTime.Value;
                TimeSpan timePassed = DateTime.Today - StartTime.Value;

                int progressTime = 0;

                double progressPercentage = (timePassed.TotalMilliseconds / totalCourseTime.TotalMilliseconds) * 100;
                if (progressPercentage > 100)
                {
                    progressPercentage = 100;
                }
                progressTime = (int)progressPercentage;

                int courseProgress = taskCount == 0 ? 0 : progress / taskCount;

                results.Add(course.Id, (progressTime, score, courseProgress));
            }
            return results;
        }
    }
}
