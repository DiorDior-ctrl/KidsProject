
namespace LessonService.Application.DTOs.Requests
{
    public record CreateCourseRequest(
        string Title,
        string Description,
        int TargetAgeMin,
        int TargetAgeMax
        );
    
}
