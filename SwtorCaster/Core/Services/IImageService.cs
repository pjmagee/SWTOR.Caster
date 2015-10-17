namespace SwtorCaster.Core.Services
{
    public interface IImageService
    {
        void Initialize();
        string GetImageById(string abilityId);
    }
}