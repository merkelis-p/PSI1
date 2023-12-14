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
        public async Task<Dictionary<int, (int ProgressTime, float Score, int courseProgress)>> GetCourseStatus(ClaimsPrincipal user)
        {
            int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            var courses = await CourseService.GetAllHierarchyAsync(userId);

            var results = new Dictionary<int, (int ProgressTime, float Score, int courseProgress)>();

            foreach (var course in courses)
            {
                Console.WriteLine($"Course:{course.Name}\n");
                float score = 0;
                int progress = 0;
                int taskCount = 0;

                foreach (var subject in course.Subjects)
                {
                    foreach (var task in subject.Tasks)
                    {
                        score += (float)task.Score * (float)task.ScoreWeight * 3.333f / 10f;
                        taskCount++;


                        if (task.Status == Enums.TaskStatus.Completed)
                        {
                            progress++;
                        }
                        Console.WriteLine($"Task: {task.Name}\n taskCount:{taskCount}; progress:{progress}");
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

                int courseProgress = taskCount == 0 ? 0 : progress * 100 / taskCount;
                
                float Score = (float)Math.Round(score, 2);
                results.Add(course.Id, (progressTime, Score, courseProgress));
            }
            return results;
        }
    }
}
