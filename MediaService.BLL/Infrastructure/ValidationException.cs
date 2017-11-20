#region usings

using System;

#endregion

namespace MediaService.BLL.Infrastructure
{
    public class ValidationException : Exception
    {
        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }

        public string Property { get; protected set; }
    }
}