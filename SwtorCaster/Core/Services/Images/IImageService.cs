namespace SwtorCaster.Core.Services.Images
{
    using System.Collections.Generic;

    public interface IImageService
    {
        void Initialize();

        string GetImageById(string abilityId);

        IEnumerable<string> GetImages();

        bool IsUnknown(string abilityId);
    }
}