using System;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalUsernameException : Exception
    {
        public IllegalUsernameException(string message) : base(message)
        {
        }
    }
}