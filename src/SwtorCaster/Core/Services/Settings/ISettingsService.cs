namespace SwtorCaster.Core.Services.Settings
{
    using Domain.Settings;

    public interface ISettingsService
    {
        AppSettings Settings { get; }
        void Save();
    }
}