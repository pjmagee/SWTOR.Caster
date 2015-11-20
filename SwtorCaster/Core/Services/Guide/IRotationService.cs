namespace SwtorCaster.Core.Services.Guide
{
    using ViewModels;
    using System.Collections.Generic;

    public interface IRotationService
    {
        RotationViewModel GetRotation(string path);
        IEnumerable<GuideRotationImage> GetGuideRotationImages();
    }
}