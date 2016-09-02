using System.Collections.Generic;

namespace SwtrorCaster.ImageMapper
{
    public class ImageMapping
    {
        public string Image { get; set; }
        public List<string> AbilityIds { get; set; }

        public ImageMapping(int id, List<string> abilityIds)
        {
            Image = $"{id}.png";
            AbilityIds = abilityIds;
        }
    }
}