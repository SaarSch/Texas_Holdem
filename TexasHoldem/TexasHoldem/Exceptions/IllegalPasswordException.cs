using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalPasswordException : Exception
    {
        public IllegalPasswordException()
        {
        }

        public IllegalPasswordException(string message) : base(message)
        {
        }

        public IllegalPasswordException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}