﻿using System;
using System.Runtime.Serialization;

namespace TexasHoldem.Exceptions
{
    [Serializable]
    internal class IllegalAvatarException : Exception
    {
        public IllegalAvatarException(string message) : base(message)
        {
        }

    }
}