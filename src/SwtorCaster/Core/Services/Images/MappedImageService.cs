namespace SwtorCaster.Core.Services.Images
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using Newtonsoft.Json;

    public class MappedImageService : IImageService
    {
        private readonly string _imagesZip = Path.Combine(Environment.CurrentDirectory, "MappedImages.zip");
        private readonly string _imagesFolder = Path.Combine(Environment.CurrentDirectory, "MappedImages");
        private readonly string _missing = Path.Combine(Environment.CurrentDirectory, "MappedImages", "missing.png");
        private List<ImageMapping> _imageMappings = new List<ImageMapping>();

        public void Initialize()
        {
            if (!Directory.Exists(_imagesFolder))
            {
                ZipFile.ExtractToDirectory(_imagesZip, Environment.CurrentDirectory);
            }

            LoadMappings();
        }

        private void LoadMappings()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SwtorCaster.Resources.mappings.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    _imageMappings = JsonConvert.DeserializeObject<List<ImageMapping>>(json);
                }
            }
        }

        public string GetImageById(long abilityId)
        {
            var mapping = _imageMappings.FirstOrDefault(x => x.AbilityIds.Contains(abilityId.ToString())) ?? new ImageMapping() { Image = "missing.png" };
            return Path.Combine(_imagesFolder, mapping.Image);
        }

        public IEnumerable<string> GetImages()
        {
            return _imageMappings.Select(x => Path.Combine(_imagesFolder, x.Image));
        }

        public bool IsUnknown(long abilityId)
        {
            return GetImageById(abilityId).Equals(_missing);
        }
    }
}