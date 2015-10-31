namespace SwtorCaster.Core.Services.Settings
{
    using Domain;
    using Domain.Settings;

    public interface ISettingsService
    {
        Settings Settings { get; }
        void Save();
    }
}