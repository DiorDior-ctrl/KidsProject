namespace LessonService.Application.Services;

public static class CacheKeys
{
    public static string AllCourses => "lessons:courses:all";
    public static string Course(Guid id) => $"lessons:course:{id}";
    public static string ModulesByCourse(Guid courseId) => $"lessons:modules:course:{courseId}";
    public static string Module(Guid id) => $"lessons:module:{id}";
    public static string LessonsByModule(Guid moduleId) => $"lessons:lessons:module:{moduleId}";
    public static string Lesson(Guid id) => $"lessons:lesson:{id}";
    public static string ExercisesByLesson(Guid lessonId) => $"lessons:exercises:lesson:{lessonId}";
    public static string Exercise(Guid id) => $"lessons:exercise:{id}";
}