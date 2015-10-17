namespace SwtorCaster.Core.Services
{
    using Domain;

    public interface ISettingsService
    {
        Settings Settings { get; }
        void Save();
    }
}