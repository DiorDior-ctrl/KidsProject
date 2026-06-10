using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Application.DTOs.Requests
{
    public record UpdateCourseRequest(
    string Title,
    string Description
);
}
