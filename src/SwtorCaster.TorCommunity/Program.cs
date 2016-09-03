using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace SwtrorCaster.ImageMapper
{
    public class Program
    {
        // Used to produce a mapped images json file to filter out duplicates by TOR Community Ability ID/Image dump.
        // The result is a unique collection of images, that map to many abilities.
        public static void Main(string[] args)
        {
            var imagesPath = args[0];
            var destination = args[1];

            Console.WriteLine("Loading images...");

            var images = Directory.GetFiles(imagesPath).Select(x => new AbilityImage(x)).ToList();

            Console.WriteLine("Analysing images...");

            var lookup = images.ToLookup(x => x.Bitmap, x => x.AbilityId, new AbilityImageComparer());

            Console.WriteLine($"Saving new images to: {destination}");

            GenerateUniqueImages(destination, lookup);

            Console.WriteLine("Done.");
            Console.Read();
        }

        private static void GenerateUniqueImages(string destination, ILookup<System.Drawing.Bitmap, string> lookup)
        {
            int i = 0;

            var abilities = new List<ImageMapping>();

            foreach (var item in lookup)
            {
                item.Key.Save($@"{destination}\{i}.png");
                abilities.Add(new ImageMapping(i, item.ToList()));
                i++;
            }

            Console.WriteLine("Serializing mappings...");

            File.WriteAllText($@"{destination}\mappings.json", JsonConvert.SerializeObject(abilities, Formatting.Indented));
        }
    }
}
