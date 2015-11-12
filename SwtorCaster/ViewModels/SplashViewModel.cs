namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;

    public class SplashViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        public SplashViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

        public void Start()
        {
            _windowManager.ShowWindow(this);
        }
    }
}