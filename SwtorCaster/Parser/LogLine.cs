namespace SwtorCaster.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class LogLine
    {
        private const string Images = "Images";
        private static readonly Regex Regex = new Regex(@"\[(.*)\] \[(.*)\] \[(.*)\] \[((.*)\s{(\d*)}?)?\] \[(.*)\s{(\d*)}:\s(.*)\s{(\d*)}\]", RegexOptions.Compiled);
        private static readonly Regex IdRegex = new Regex(@"\s*\{\d*}\s*", RegexOptions.Compiled);

        private static readonly Dictionary<string, string> Files;
        private static readonly string[] SplitOptions = { "," };
        private static readonly string Missing = Path.Combine(Environment.CurrentDirectory, Images, "missing.png");

        static LogLine()
        {
            Files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, Images))
                .ToDictionary(k => Path.GetFileNameWithoutExtension(k).ToLower(), value => value);
        }

        public string Id { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string EventType { get; set; }
        public string EventDetail { get; set; }
        public string AbilityText => Settings.Current.EnableAliases ? Aliases[App.Random.Next(0, Aliases.Length)] : Ability;
        public string Ability { get; set; }

        public string[] Aliases
        {
            get
            {
                try
                {
                    var ability = Settings.Current.Abilities.FirstOrDefault(a => a.Name.Equals(Id, StringComparison.OrdinalIgnoreCase));

                    if (ability != null)
                    {
                        return ability.Aliases.Split(SplitOptions, StringSplitOptions.None);
                    }
                }
                catch (Exception e)
                {
                    if (Settings.Current.EnableLogging)
                    {
                        File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), $"Missing image for {Ability}. {Environment.NewLine}");
                    }
                }

                return new[] { Ability };
            }
        }

        public string ImageUrl
        {
            get
            {
                try
                {
                    return Files[Id];
                }
                catch
                {
                    File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), $"Missing image for {Ability}. {Environment.NewLine}");
                }

                return Missing;
            }
        }

        public LogLine(string line)
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