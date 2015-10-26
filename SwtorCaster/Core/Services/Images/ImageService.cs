namespace SwtorCaster.Core.Services.Images
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using Logging;

    public class ImageService : IImageService
    {
        private readonly ILoggerService _loggerService;
        private IDictionary<string, string> _files;

        private readonly string _imagesZip = Path.Combine(Environment.CurrentDirectory, "Images.zip");
        private readonly string _imagesFolder = Path.Combine(Environment.CurrentDirectory, "Images");
        private readonly string _missing = Path.Combine(Environment.CurrentDirectory, "Images", "missing.png");

        public ImageService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public string GetImageById(string abilityId)
        {
            try
            {
                return _files[abilityId];
            }
            catch (Exception e)
            {
                _loggerService.Log(e.Message);
            }

            return _missing;
        }

        public IEnumerable<string> GetImages()
        {
            return Directory.EnumerateFiles(_imagesFolder, "*.png", SearchOption.AllDirectories);
        }

        public bool IsUnknown(string abilityId)
        {
            return !_files.ContainsKey(abilityId);
        }

        public void Initialize()
        {
            if (!Directory.Exists(_imagesFolder) || !Directory.EnumerateFiles(_imagesFolder).Any())
            {
                _loggerService.Log($"Extracting Images.zip for Ability Window");
                ZipFile.ExtractToDirectory(_imagesZip, Environment.CurrentDirectory);
            }

            _files = Directory.GetFiles(_imagesFolder)
                .ToDictionary(k => Path.GetFileNameWithoutExtension(k).ToLower(), value => value);
        }
    }
}