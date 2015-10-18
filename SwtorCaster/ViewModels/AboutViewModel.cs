namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using Screens;

    public class AboutViewModel : FocusableScreen
    {
        public override string DisplayName { get; set; } = "SWTOR Caster - About";

        public AboutViewModel()
        {
        }

        public void Focus()
        {
            var window = GetView() as Window;
            window?.Activate();
        }
    }
}