namespace SwtorCaster.Core.Services.Settings
{
    using Domain.Settings;

    public interface ISettingsService
    {
        Settings Settings { get; }
        void Save();
    }
}