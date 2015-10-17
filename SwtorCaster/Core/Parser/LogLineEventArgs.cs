namespace SwtorCaster.Core.Parser
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows;
    using Caliburn.Micro;
    using Services;

    public class LogLineEventArgs : PropertyChangedBase
    {
        public static readonly Random Random = new Random();

        private IImageService ImageService => IoC.Get<IImageService>();
        private ISettingsService SettingsService => IoC.Get<ISettingsService>();

        private static readonly Regex Regex = new Regex(@"\[(.*)\] \[(.*)\] \[(.*)\] \[((.*)\s{(\d*)}?)?\] \[(.*)\s{(\d*)}:\s(.*)\s{(\d*)}\]", RegexOptions.Compiled);

        public string Id { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string EventType { get; set; }
        public string EventDetail { get; set; }
        public string Ability { get; set; }
        public string ImageUrl => ImageService.GetImageById(Id);
        public int Angle => Random.Next(-SettingsService.Settings.Rotate, SettingsService.Settings.Rotate);
        public Visibility EnableAbilityName => SettingsService.Settings.EnableAbilityText ? Visibility.Visible : Visibility.Hidden;

        public LogLineEventArgs(string line)
        {
            var match = Regex.Match(line);

            if (match.Success)
            {
                Ability = match.Groups[5].Value;
                Id = match.Groups[6].Value;
                EventType = match.Groups[7].Value;
                EventDetail = match.Groups[9].Value;
            }
        }
    }
}