namespace SwtorCaster
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Windows;
    using Parser;

    public partial class App
    {
        private readonly string imagesZip = Path.Combine(Environment.CurrentDirectory, "Images.zip");
        private readonly string imagesFolder = Path.Combine(Environment.CurrentDirectory, "Images");

        public static readonly Random Random = new Random();
        public static int ImageAngle => Random.Next(Settings.Current.MinimumAngle, Settings.Current.MaximumAngle);
        public static Visibility EnableAbilityName => Settings.Current.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;

        protected override void OnStartup(StartupEventArgs e)
        {
            var splash = new SplashScreen("Resources/splash.jpg");
            splash.Show(false);

            if (!Directory.Exists(imagesFolder))
            {
                File.WriteAllLines(Settings.LogPath, new[] {$"[{DateTime.Now}] Extracting Images.zip for Ability Window. {Environment.NewLine}"});

                ZipFile.ExtractToDirectory(imagesZip, Environment.CurrentDirectory);
            }

            splash.Close(TimeSpan.FromSeconds(0));
        }

        protected override void OnExit(ExitEventArgs e)
        {

        }
    }
}
