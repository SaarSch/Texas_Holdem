using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalPasswordException : Exception
    {
        public IllegalPasswordException(string message) : base(message)
        {
        }

    }
}