namespace SwtorCaster.Core.Services.Settings
{
    using Domain;

    public interface ISettingsService
    {
        Settings Settings { get; }
        void Save();
    }
}