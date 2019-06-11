using System;

namespace Projects.BLL.Infrastructure.Exceptions
{
    public class HasRelationsException : Exception
    {
        static string message = "Item has relations.";

        public HasRelationsException() : base(message) { }
    }
}
