namespace SwtorCaster.Core
{
    using System;

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
