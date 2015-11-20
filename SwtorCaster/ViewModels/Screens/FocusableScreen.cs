using System.Windows;
using Caliburn.Micro;

namespace SwtorCaster.ViewModels
{
    public class FocusableScreen : Screen
    {
        public void Focus()
        {
            var window = GetView() as Window;
            window?.Activate();
        }
    }
}