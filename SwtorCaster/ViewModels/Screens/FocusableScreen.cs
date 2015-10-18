namespace SwtorCaster.ViewModels.Screens
{
    using System.Windows;
    using Caliburn.Micro;

    public class FocusableScreen : Screen
    {
        public void Focus()
        {
            var window = GetView() as Window;
            window?.Activate();
        }
    }
}