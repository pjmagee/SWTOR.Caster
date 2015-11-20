namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;

    public class RotationViewModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public string Website { get; set; }

        public BindableCollection<RotationItemViewModel>  RotationItems { get; set; } = new BindableCollection<RotationItemViewModel>();
        
        public void AddNewItem()
        {
            RotationItems.Add(new RotationItemViewModel(this));
        }
    }
}