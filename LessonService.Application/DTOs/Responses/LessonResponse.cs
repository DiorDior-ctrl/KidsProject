using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Application.DTOs.Responses
{
    public record LessonResponse(
    Guid Id,
    Guid ModuleId,
    string Title,
    int OrderIndex,
    int XpReward,
    bool HasVideo,
    DateTime CreatedAt
    );
}
