using System;

namespace Projects.BLL.Infrastructure.Exceptions
{
    public class OldPasswordIsWrongException : Exception
    {
        static string message = "Old password is incorrect.";

        public OldPasswordIsWrongException() : base(message) { }
    }
}
