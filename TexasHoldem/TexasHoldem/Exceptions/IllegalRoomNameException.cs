using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalRoomNameException : Exception
    {
        public IllegalRoomNameException()
        {
        }

        public IllegalRoomNameException(string message) : base(message)
        {
        }

        public IllegalRoomNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalRoomNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}