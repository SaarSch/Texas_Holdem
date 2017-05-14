using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalAvatarException : Exception
    {
        public IllegalAvatarException()
        {
        }

        public IllegalAvatarException(string message) : base(message)
        {
        }

        public IllegalAvatarException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalAvatarException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}