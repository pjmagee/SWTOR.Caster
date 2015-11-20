namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;

    public class GuideSettingsViewModel : PropertyChangedBase
    {
        public LoadGuideViewModel LoadGuideViewModel { get; }
        public CreateGuideViewModel CreateGuideViewModel { get; }

        public GuideSettingsViewModel(LoadGuideViewModel loadGuideViewModel, CreateGuideViewModel createGuideViewModel)
        {
            LoadGuideViewModel = loadGuideViewModel;
            CreateGuideViewModel = createGuideViewModel;
        }
    }
}