using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WakeyWakey.Services
{
    [Authorize]
    public class SubjectStatusService
    {
        private ICourseApiService CourseService;
        public SubjectStatusService(ICourseApiService courseService)
        {
            CourseService = courseService;
        }

        //public async Task<float> UpdateSubjectStatuses()

        public async Task<Dictionary<int, (int ProgressTime, float Score, int subjectProgress)>> GetSubjectStatus(ClaimsPrincipal user)
        {
            int userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
            var courses = await CourseService.GetAllHierarchyAsync(userId);

            var results = new Dictionary<int, (int ProgressTime, float Score, int subjectProgress)>();

            foreach (var course in courses)
            {
                foreach (var subject in course.Subjects)
                {
                    float score = 0;
                    int progress = 0;
                    int taskCount = 0;

                    foreach (var task in subject.Tasks)
                    {
                        score += (float)task.Score * (float) task.ScoreWeight / 10;

                        taskCount++;

                        if (task.Status == Enums.TaskStatus.Completed)
                        {
                            progress++;
                        }
                    }

                    int subjectProgress = taskCount == 0 ? 0 : progress * 100 / taskCount;

                    DateTime? StartTime = subject.StartDate;
                    DateTime? EndTime = subject.EndDate;

                    TimeSpan totalSubjectTime = EndTime.Value - StartTime.Value;
                    TimeSpan timePassed = DateTime.Today - StartTime.Value;

                    int progressTime = 0;

                    double progressPercentage = (timePassed.TotalMilliseconds / totalSubjectTime.TotalMilliseconds) * 100;
                    if (progressPercentage > 100)
                    {
                        progressPercentage = 100;
                    }
                    progressTime = (int)progressPercentage;

                    //Console.WriteLine($"ProgressTime: {progressTime} progressPercentage: {progressPercentage}");

                    results.Add(subject.Id, (progressTime, score, subjectProgress));
                }
            }

            return results;
        }

    }
}
