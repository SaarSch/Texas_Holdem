using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalCriteriaException : Exception
    {
        public IllegalCriteriaException()
        {
        }

        public IllegalCriteriaException(string message) : base(message)
        {
        }

        public IllegalCriteriaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalCriteriaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}