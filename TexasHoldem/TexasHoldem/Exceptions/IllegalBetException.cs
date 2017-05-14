using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalBetException : Exception
    {
        public IllegalBetException()
        {
        }

        public IllegalBetException(string message) : base(message)
        {
        }

        public IllegalBetException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalBetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}