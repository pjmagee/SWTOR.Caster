using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SwtorCaster.Core.Services.Images;

namespace SwtorCaster.Core.Services.Ability
{
    public class ApiAbilitySearch : IAbilityService
    {
        private readonly IImageService _imageService;

        private static readonly string[] Classes =
        {
            "abl.sith_warrior",
            "abl.sith_inquisitor",
            "abl.agent",
            "abl.bounty_hunter",
            "abl.jedi_knight",
            "abl.jedi_consular",
            "abl.smuggler",
            "abl.trooper"
        };

        public ApiAbilitySearch(IImageService imageService)
        {
            _imageService = imageService;
        }

        public IEnumerable<AbilityItem> GetAbilities()
        {
            throw new NotImplementedException();
        }

        public AbilityItem GetById(string id)
        {
            throw new NotImplementedException();
        }

        public AbilityItem GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AbilityItem> GetByFilter(string search)
        {
            using (var client = new WebClient())
            {
                var json =  client.DownloadString($"http://dedi.rl-web.no/ability?name={search}");

                var abilities = (JArray)JsonConvert.DeserializeObject(json);

                return abilities.Where(x => IsPlayerClass(x["Fqn"].ToString())).Select(item => new AbilityItem
                {
                    Id = item["NameId"].ToString(),
                    Name = item["Name"].ToString(),
                    Description = item["Description"].ToString(),
                    Image = _imageService.GetImageById(long.Parse(item["NameId"].ToString())),
                });
            }
        }

        private bool IsPlayerClass(string fqn)
        {
            return Classes.Any(baseClass => fqn.Contains(baseClass));
        }
    }
}