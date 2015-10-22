using System.Collections;
using System.Collections.Generic;

namespace SwtorCaster.Core.Services
{
    public interface IImageService
    {
        void Initialize();

        string GetImageById(string abilityId);

        IEnumerable<string> GetImages();
    }
}