using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalLeagueException : Exception
    {
        public IllegalLeagueException()
        {
        }

        public IllegalLeagueException(string message) : base(message)
        {
        }

        public IllegalLeagueException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IllegalLeagueException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}