using System;

namespace Projects.BLL.Infrastructure.Exceptions
{
    public class NotFoundException : Exception
    {
        static string message = "Item with given id was not found.";

        public NotFoundException() : base(message) { }
    }
}
