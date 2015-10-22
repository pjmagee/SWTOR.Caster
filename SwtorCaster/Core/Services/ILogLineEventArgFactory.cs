using SwtorCaster.Core.Parser;

namespace SwtorCaster.Core.Services
{
    public interface ILogLineEventArgFactory
    {
        LogLineEventArgs Create(string line);
    }
}