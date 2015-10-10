namespace SwtorCaster
{
    using System;
    using System.Windows;
    using Parser;
    using ViewModel;

    public partial class App
    {
        public static readonly Random Random = new Random();
        public static int ImageAngle => Random.Next(Settings.Current.MinimumAngle, Settings.Current.MaximumAngle);
        public static Visibility EnableAbilityName => Settings.Current.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;

        protected override void OnStartup(StartupEventArgs e)
        {
            var splash = new SplashScreen("Resources/splash.jpg");
            splash.Show(autoClose: false);
            splash.Close(TimeSpan.FromSeconds(20));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ViewModelLocator.Cleanup();
        }
    }
}
