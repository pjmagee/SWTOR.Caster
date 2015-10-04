namespace SwtorCaster
{
    using System;
    using System.Configuration;
    using System.Windows;

    public partial class App : Application
    {
        private static readonly Random Random = new Random();
        public static Visibility EnableAbilityName => ConfigurationManager.AppSettings["swtor.enable.ability.name"] == "true" ? Visibility.Visible : Visibility.Hidden;
        public static int ImageAngle => Random.Next(-15, 15);
    }
}
