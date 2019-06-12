using System;

namespace Projects.BLL.Infrastructure.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        static string message = "User already exists.";

        public UserAlreadyExistsException() : base(message) { }
    }
}
