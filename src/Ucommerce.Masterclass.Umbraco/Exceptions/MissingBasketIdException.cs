using System;
using System.Diagnostics.CodeAnalysis;

namespace MC_Headless.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public sealed class MissingBasketIdException : Exception
    {
        public MissingBasketIdException(string message)
            : base(message)
        {
        }

        public MissingBasketIdException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}