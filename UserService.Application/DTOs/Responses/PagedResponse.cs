using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Application.DTOs.Responses
{
    public record PagedResponse<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int Page,
        int PageSize,
        int TotalPages);
    
}
