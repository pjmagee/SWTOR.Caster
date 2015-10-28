namespace SwtorCaster.Core.Services.Parsing
{
    using Domain;

    public interface ILogLineParser
    {
        LogLine Parse(string line);
    }
}