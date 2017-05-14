using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalCardValueException : Exception
    {
        public IllegalCardValueException()
        {
        }

        public IllegalCardValueException(string message) : base(message)
        {
        }

        public IllegalCardValueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalCardValueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}