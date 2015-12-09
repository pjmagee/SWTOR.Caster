using System;
using System.Threading.Tasks;

namespace SwtorCaster.Core.Services.Ability
{
    using System.Collections.Generic;
    using Images;

    public class TemporaryAbilityService : IAbilityService
    {
        private readonly IImageService _imageService;

        public TemporaryAbilityService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public IEnumerable<AbilityItem> GetAbilities()
        {
            yield return new AbilityItem() { Name = "Massacre", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Ravage", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Beserk", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Recklessness", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Force Charge", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Force Scream", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Jet Charge", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Vengeful Slam", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Force Push", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Force Choke", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Deadly Sabers", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Gore", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Annilate", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Rupture", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Force Rend", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
            yield return new AbilityItem() { Name = "Force Smash", Id = "1488914837667840", Image = _imageService.GetImageById(1488914837667840) };
        }

        public AbilityItem GetById(string id)
        {
            return new AbilityItem() { Name = "Devastating Blast", Image = _imageService.GetImageById(1488914837667840) };
        }

        public AbilityItem GetByName(string name)
        {
            return new AbilityItem() { Name = "Heroic Moment", Image = _imageService.GetImageById(1488914837667840) };
        }

        public Task<IEnumerable<AbilityItem>> GetByFilter(string search)
        {
            throw new NotImplementedException();
        }

        IEnumerable<AbilityItem> IAbilityService.GetByFilter(string search)
        {
            throw new NotImplementedException();
        }
    }
}
