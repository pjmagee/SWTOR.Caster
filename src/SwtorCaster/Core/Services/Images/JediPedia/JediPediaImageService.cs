using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Newtonsoft.Json;

namespace SwtorCaster.Core.Services.Images.JediPedia
{
    /// <summary>
    /// JediPedia implementation allows us to find an image in a file 
    /// </summary>
    public class JediPediaImageService : IImageService
    {
        private const string JediPediaImagesFolderName = "JediPediaImages";
        private readonly string imagesZip = Path.Combine(Environment.CurrentDirectory, "JediPediaImages.zip");
        private readonly string imagesFolder = Path.Combine(Environment.CurrentDirectory, JediPediaImagesFolderName);
        private readonly string missingImage = Path.Combine(Environment.CurrentDirectory, JediPediaImagesFolderName, "default.png");
        private readonly string abilitiesJsonFile = Path.Combine(Environment.CurrentDirectory, JediPediaImagesFolderName, "abilities.json");
        private List<Ability> abilities = new List<Ability>();

        public void Initialize()
        {
            if (!Directory.Exists(imagesFolder))
            {
                ZipFile.ExtractToDirectory(imagesZip, Environment.CurrentDirectory);
            }

            var json = File.ReadAllText(abilitiesJsonFile);

            abilities = JsonConvert.DeserializeObject<IEnumerable<Ability>>(json).ToList();
        }

        public string GetImageById(long abilityId)
        {
            var ability = abilities.FirstOrDefault(x => x.Id == abilityId.ToString());
            return ability != null ? Path.Combine(imagesFolder, ability.IconName + ".png") : missingImage;
        }

        public IEnumerable<string> GetImages()
        {
            return abilities.Select(x => Path.Combine(imagesFolder, x.IconName));
        }

        public bool IsUnknown(long abilityId)
        {
            return GetImageById(abilityId).Equals(missingImage);
        }
    }
}