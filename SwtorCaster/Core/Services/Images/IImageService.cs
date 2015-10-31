namespace SwtorCaster.Core.Services.Images
{
    using System.Collections.Generic;

    public interface IImageService
    {
        void Initialize();
        string GetImageById(long abilityId);
        IEnumerable<string> GetImages();
        bool IsUnknown(long abilityId);
    }
}