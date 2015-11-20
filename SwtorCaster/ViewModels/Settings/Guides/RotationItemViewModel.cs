namespace SwtorCaster.ViewModels
{
    public class RotationItemViewModel
    {
        private RotationViewModel RotationViewModel { get; }

        public RotationItemViewModel(RotationViewModel rotationViewModel)
        {
            RotationViewModel = rotationViewModel;
        }

        public string AbilityId { get; set; }
        public string AbilityName { get; set; }
        public string HelpText { get; set; }
        public string ImageUrl { get; set; }

        public void Delete()
        {
            RotationViewModel.RotationItems.Remove(this);
        }
    }
}