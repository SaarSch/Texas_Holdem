using System;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalCardValueException : Exception
    {
        public IllegalCardValueException(string message) : base(message)
        {
        }
    }
}