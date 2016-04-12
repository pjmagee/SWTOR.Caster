namespace SwtorCaster.ViewModels
{
    using Caliburn.Micro;

    public class SplashViewModel : Screen
    {
        private readonly IWindowManager windowManager;

        public SplashViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public void Start()
        {
            windowManager.ShowWindow(this);
        }
    }
}