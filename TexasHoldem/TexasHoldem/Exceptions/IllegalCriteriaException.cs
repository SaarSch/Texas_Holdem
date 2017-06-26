using System;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalCriteriaException : Exception
    {
        public IllegalCriteriaException(string message) : base(message)
        {
        }
    }
}