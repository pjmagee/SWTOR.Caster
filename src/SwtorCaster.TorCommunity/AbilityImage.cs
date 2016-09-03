using System.Drawing;
using System.IO;

namespace SwtrorCaster.ImageMapper
{
    public class AbilityImage
    {
        public string File { get; set; }
        public Bitmap Bitmap { get; set; }
        public string AbilityId { get; set; }

        public AbilityImage(string file)
        {
            AbilityId = Path.GetFileNameWithoutExtension(file);
            File = file;
            Bitmap = new Bitmap(file);
        }
    }
}