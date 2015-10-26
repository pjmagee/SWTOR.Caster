namespace SwtorCaster.Core.Factories
{
    using Domain;

    public interface ILogLineFactory
    {
        LogLine Create(string line);
    }
}