using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalUsernameException : Exception
    {
        public IllegalUsernameException()
        {
        }

        public IllegalUsernameException(string message) : base(message)
        {
        }

        public IllegalUsernameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalUsernameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}