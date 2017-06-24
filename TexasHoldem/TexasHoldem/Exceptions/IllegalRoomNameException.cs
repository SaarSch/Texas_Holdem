using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalRoomNameException : Exception
    {

        public IllegalRoomNameException(string message) : base(message)
        {
        }

    }
}