

namespace UserService.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message){}
        public NotFoundException(string entityName , Guid id)
            : base($"{entityName} me id '{id}'nuk u gjet.")
        {
            
        }
    }
}
