using System;

namespace SwtorCaster.Core.Services.Guide
{
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;
    using ViewModels;
    using Domain.Guide;
    using System.Linq;
    using Caliburn.Micro;
    using Images;

    public class RotationService : IRotationService
    {
        private readonly IImageService _imageService;

        public RotationService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public RotationViewModel GetRotation(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                var rotation = JsonConvert.DeserializeObject<Rotation>(json);

                var rotationViewModel = new RotationViewModel
                {
                    Website = rotation.Website,
                    Author = rotation.Author,
                    Description = rotation.Description,
                    Title = rotation.Title,
                    Version = rotation.Version
                };

                rotationViewModel.RotationItems = new BindableCollection<RotationItemViewModel>(rotation.RotationItems.Select(x =>
                    new RotationItemViewModel(rotationViewModel)
                    {
                        AbilityId = x.AbilityId,
                        AbilityName = x.Text,
                        HelpText = x.Tooltip,
                        ImageUrl = _imageService.GetImageById(long.Parse(x.AbilityId))
                    }));

                return rotationViewModel;

            }
            catch (Exception e)
            {
                return new RotationViewModel();
            }
        }

        public IEnumerable<GuideRotationImage> GetGuideRotationImages()
        {
            return _imageService.GetImages().Select(x => new GuideRotationImage()
            {
                AbilityId = Path.GetFileNameWithoutExtension(x),
                ImageUrl = x
            });
        }
    }
}