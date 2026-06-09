using System;
using System.Collections.Generic;
using System.Text;

namespace LessonService.Domain.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("Nuk ke leje per kete veprim.")
        {

        }
    }
}
