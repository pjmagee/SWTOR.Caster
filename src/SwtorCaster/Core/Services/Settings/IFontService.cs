namespace SwtorCaster.Core.Services.Settings
{
    using System.Collections.Generic;
    using System.Windows.Media;

    public interface IFontService
    {
        string FontsPath { get; }

        IEnumerable<FontFamily> GetSelectableFonts();

        FontFamily GetFontFromString(string value);

        string GetStringFromFont(FontFamily family);
        void Initialize();
    }
}