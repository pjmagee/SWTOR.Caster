namespace SwtorCaster.JediPedia
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Newtonsoft.Json;

    public class Program
    {
        /// <summary>
        /// Downloads the latest abilities JSON file. Restructures the json to only require the minimum amount needed, serialises and saves to file along wth downloading all images.
        /// JediPedia approach only provides unique images, so does not need image mapping like the TorCommunity approach.
        /// </summary>
        static void Main(string[] args)
        {
            var authKey = args[0];
            var directory = args[1];

            using (var client = new WebClient())
            {
                var json = client.DownloadString($"https://swtor.jedipedia.net/ajax/getAbilities.php?auth={authKey}");

                var abilities = JsonConvert.DeserializeObject<IEnumerable<Ability>>(json);

                File.WriteAllText(Path.Combine(directory, "abilities.json"), JsonConvert.SerializeObject(abilities, Formatting.Indented));

                Dictionary<string, Image> abilityIcons = new Dictionary<string, Image>();

                ImageCodecInfo info = ImageCodecInfo.GetImageDecoders().First(x => x.FormatID == ImageFormat.Png.Guid);

                using (var encoderParameters = new EncoderParameters(1))
                {
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

                    foreach (var ability in abilities)
                    {
                        if (!abilityIcons.ContainsKey(ability.IconName))
                        {
                            var data = client.DownloadData($"https://swtorfiles.jedipedia.net/icons/56/{ability.IconName}.png");
                            var image = Image.FromStream(new MemoryStream(data));
                            var fileName = string.IsNullOrEmpty(ability.IconName) || ability.IconName == "*" ? "missing" : ability.IconName;
                            var path = Path.Combine(directory, fileName + ".png");
                            abilityIcons.Add(ability.IconName, image);

                            // Save with configured image quality
                            image.Save(path, info, encoderParameters);
                        }
                    }
                }
            }
        }
    }
}
