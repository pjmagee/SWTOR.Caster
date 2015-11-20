namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;
    using Microsoft.Win32;
    using Core.Domain.Guide;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;
    using Core.Services.Guide;
    using SwtorCaster.Core.Services.Images;

    public class CreateGuideViewModel : PropertyChangedBase
    {
        public string GuideTitle { get; set; }

        public string GuideDescription { get; set; }

        public string GuideAuthor { get; set; }

        public string GuideVersion { get; set; }

        public string GuideLink { get; set; }

        private readonly IRotationService _rotationService;

        public RotationViewModel RotationViewModel { get; set; } = new RotationViewModel();

        public CreateGuideViewModel(IRotationService rotationService, IImageService imageService)
        {
            _rotationService = rotationService;
            Images = new BindableCollection<GuideImageItem>(imageService.GetImages().Select(x => new GuideImageItem(x)));
        }

        public BindableCollection<GuideImageItem> Images { get; }

        public BindableCollection<RotationItemViewModel> RotationItems => RotationViewModel?.RotationItems;

        public void SaveGuide()
        {
            SaveFileDialog fileDialog = new SaveFileDialog()
            {
                Title = "Save SWTOR Caster Guide",
                AddExtension = true,
                DefaultExt = ".scg",
                OverwritePrompt = true,
                ValidateNames = true,
                Filter = "SWTOR Caster Guide file (*.scg)|*.scg;",
            };

            var showDialog = fileDialog.ShowDialog();

            if (showDialog.GetValueOrDefault())
            {
                var rotation = new Rotation
                {
                    Title = GuideTitle,
                    Description = GuideDescription,
                    Author = GuideAuthor,
                    Website = GuideLink,
                    Version = GuideVersion,
                    RotationItems = RotationItems
                        .Select(x => new RotationItem() { AbilityId = x.AbilityId, Text = x.AbilityName, Tooltip = x.HelpText })
                        .ToList()
                };

                var json = JsonConvert.SerializeObject(rotation);

                File.WriteAllText(fileDialog.FileName, json);
            }
        }

        public void LoadGuide()
        {
            FileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Guide file (*.scg)|*.scg;",
                Title = "Select guide file",
                CheckPathExists = true,
                CheckFileExists = true
            };

            var result = fileDialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                RotationViewModel = _rotationService.GetRotation(fileDialog.FileName);
                Load();
            }
        }

        public void AddRotationItem()
        {
            RotationViewModel.AddNewItem();
            Refresh();
        }

        private void Load()
        {
            GuideTitle = RotationViewModel.Title;
            GuideDescription = RotationViewModel.Description;
            GuideAuthor = RotationViewModel.Author;
            GuideVersion = RotationViewModel.Version;
            GuideLink = RotationViewModel.Website;
            Refresh();
        }
    }
}