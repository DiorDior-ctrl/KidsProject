using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Application.DTOs.Responses
{
    public record CourseResponse(
    Guid Id,
    string Title,
    string Description,
    int TargetAgeMin,
    int TargetAgeMax,
    bool IsActive,
    DateTime CreatedAt
    );
}
