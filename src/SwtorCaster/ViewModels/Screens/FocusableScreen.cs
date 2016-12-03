namespace SwtorCaster.ViewModels
{
    using System.Windows;
    using Caliburn.Micro;

    public class FocusableScreen : Screen
    {
        public void Focus() => Window?.Activate();

        protected Window Window => GetView() as Window;        
    }
}