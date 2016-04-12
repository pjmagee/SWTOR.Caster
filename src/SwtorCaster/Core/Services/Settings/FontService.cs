using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using SwtorCaster.Core.Extensions;

namespace SwtorCaster.Core.Services.Settings
{
    public class FontService : IFontService
    {
        private static readonly string SwtorCaster = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SwtorCaster");
        private static readonly string FontsFolder = Path.Combine(SwtorCaster, "fonts");
        private IEnumerable<FontFamily> fontFamilies;
        private static readonly FontFamily DefaultFontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Resources/#SF Distant Galaxy");

        public string FontsPath => FontsFolder;

        public IEnumerable<FontFamily> GetSelectableFonts() => fontFamilies;

        public FontFamily GetFontFromString(string value) => fontFamilies.FirstOrDefault(x => x.FamilyNames.Values.Contains(value)) ?? DefaultFontFamily;

        public string GetStringFromFont(FontFamily family) => family.GetFamilyName();

        public void Initialize()
        {
            if (!Directory.Exists(FontsFolder))
            {
                Directory.CreateDirectory(FontsFolder);
            }

            fontFamilies = new DirectoryInfo(FontsFolder)
                .EnumerateFiles("*.ttf")
                .Select(x => new { Uri = new Uri(x.Directory.FullName + "/"), FamilyName = new GlyphTypeface(new Uri(x.FullName)).GetFamilyName() })
                .Select(x => new FontFamily(x.Uri, "./#" + x.FamilyName))
                .Concat(Fonts.SystemFontFamilies.ToList())
                .Concat(new [] { DefaultFontFamily })
                .OrderBy(x => x.GetFamilyName())
                .Distinct(new FontFamilyNameComparer())
                .ToList();
        }

        class FontFamilyNameComparer : IEqualityComparer<FontFamily>
        {
            public bool Equals(FontFamily x, FontFamily y)
            {
                if (x == null) return false;
                if (y == null) return false;
                return x.GetFamilyName().Equals(y.GetFamilyName());
            }

            public int GetHashCode(FontFamily obj)
            {
                return obj.GetFamilyName().GetHashCode();
            }
        }
    }



}