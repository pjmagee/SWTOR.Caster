namespace SwtorCaster
{
    using System;
    using System.Configuration;
    using System.Windows;

    public partial class App : Application
    {
        public static string[] Keys => ConfigurationManager.AppSettings.AllKeys;

        public static readonly Random Random = new Random();
        public static int ImageAngle => Random.Next(MinAngle, MaxAngle);

        public static int MinAngle => int.Parse(ConfigurationManager.AppSettings["swtor.image.min.rotate"]);
        public static int MaxAngle => int.Parse(ConfigurationManager.AppSettings["swtor.image.max.rotate"]);
        public static int MaxItems => int.Parse(ConfigurationManager.AppSettings["swtor.max.items"]);

        public static Visibility EnableAbilityName => ConfigurationManager.AppSettings["swtor.enable.ability.text"] == "true" ? Visibility.Visible : Visibility.Hidden;
        public static bool EnableLog => ConfigurationManager.AppSettings["swtor.enable.log"] == "true";
        public static bool EnableAliases => ConfigurationManager.AppSettings["swtor.enable.aliases"] == "true";
        public static bool EnableExitCombatClear => ConfigurationManager.AppSettings["swtor.enable.combat.clear"] == "true";

        protected override void OnStartup(StartupEventArgs e)
        {
            var splash = new SplashScreen("Resources/splash.jpg");
            splash.Show(autoClose: false);
            splash.Close(TimeSpan.FromSeconds(10));
        }
    }
}
