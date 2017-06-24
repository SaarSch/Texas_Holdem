using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalLeagueException : Exception
    {
        public IllegalLeagueException(string message) : base(message)
        {
        }

    }
}