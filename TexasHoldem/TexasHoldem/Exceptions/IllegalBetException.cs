using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalBetException : Exception
    {
        public IllegalBetException(string message) : base(message)
        {
        }
    }
}