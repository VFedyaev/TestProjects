using System;

namespace Projects.BLL.Infrastructure.Exceptions
{
    public class InsecurePasswordException : Exception
    {
        static string message = "Password is insecure.";

        public InsecurePasswordException() : base(message) { }
    }
}
