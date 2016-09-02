namespace SwtorCaster.Core
{
    using System;

    [Serializable]
    public class ParseException : ApplicationException
    {
        public ParseException(string message) : base(message)
        {
        }

        public ParseException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
