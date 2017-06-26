using System;

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