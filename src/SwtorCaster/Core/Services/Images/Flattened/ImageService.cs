namespace SwtorCaster.Core.Services.Images.Flattened
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using SwtorCaster.Core.Services.Logging;

    /// <summary>
    /// Default flattend images implemenetation finds an image file with the name of the ability ID.
    /// </summary>
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

        public string GetImageById(long abilityId)
        {
            try
            {
                return _files[abilityId.ToString()];
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

        public bool IsUnknown(long abilityId)
        {
            return !_files.ContainsKey(abilityId.ToString());
        }

        public void Initialize()
        {
            if (!Directory.Exists(_imagesFolder) || !Directory.EnumerateFiles(_imagesFolder).Any())
            {
                _loggerService.Log($"Extracting Images.zip");
                ZipFile.ExtractToDirectory(_imagesZip, Environment.CurrentDirectory);
            }

            _files = Directory.GetFiles(_imagesFolder)
                .ToDictionary(k => Path.GetFileNameWithoutExtension(k).ToLower(), value => value);
        }
    }
}