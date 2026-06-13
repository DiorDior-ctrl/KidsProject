using System;
using System.Collections.Generic;
using System.Text;

namespace GamificationService.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
        public NotFoundException(string entityName, Guid id)
            : base($"{entityName} ke id '{id} nuk u gjet.'")
        {

        }
    }
}
