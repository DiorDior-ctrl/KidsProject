using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Application.DTOs.Requests
{
    public record CreateLessonRequest(
    Guid ModuleId,
    string Title,
    int OrderIndex,
    int XpReward
    );
}
