using System;
using System.Runtime.Serialization;

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