using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Application.DTOs.Requests
{
    public record CreateModuleRequest(
    Guid CourseId,
    string Title,
    int OrderIndex
    );
}
