using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Application.DTOs.Responses
{
    public record ModuleResponse(
    Guid Id,
    Guid CourseId,
    string Title,
    int OrderIndex,
    DateTime CreatedAt
    );
}
