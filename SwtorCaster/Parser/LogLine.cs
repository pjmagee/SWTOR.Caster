namespace SwtorCaster.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class LogLine
    {
        private const string Images = "Images";
        private static readonly Regex Regex = new Regex(@"\[(.*)\] \[(.*)\] \[(.*)\] \[(.*)\] \[(.*)\] \((.*)\)[.<]*([!>]*)[\s<]*(\d*)?[>]*", RegexOptions.Compiled);
        private static readonly Regex IdRegex = new Regex(@"\s*\{\d*}\s*", RegexOptions.Compiled);

        private static readonly Dictionary<string, string> Files;
        private static readonly string[] SplitOptions = { "," };
        private static readonly string Missing = Path.Combine(Environment.CurrentDirectory, Images, "missing.png");

        static LogLine()
        {
            Files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, Images))
                             .ToDictionary(k => Path.GetFileNameWithoutExtension(k), v => v);
        }

        public DateTime TimeStamp { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string RandomAlias => App.EnableAliases ? Aliases[App.Random.Next(0, Aliases.Length)] : Ability;
        public string Ability { get; set; }

        public string[] Aliases
        {
            get
            {
                try
                {
                    if (App.Keys.Contains(Ability, StringComparer.OrdinalIgnoreCase))
                    {
                        var aliases = ConfigurationManager.AppSettings[Ability.ToLower()];
                        return aliases.Split(SplitOptions, StringSplitOptions.None);
                    }
                }
                catch (Exception e)
                {
                    if (App.EnableLog)
                    {
                        File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), $"Missing image for {Ability}. {Environment.NewLine}");
                    }
                }

                return new[] {Ability};
            }
        }

        public string EventType { get; set; }
        public string EventDetail { get; set; }
        public bool CritValue { get; set; }
        public int Value { get; set; }
        public string ValueType { get; set; }
        public int Threat { get; set; }

        public string ImageUrl
        {
            get
            {
                try
                {
                    return Files[Ability.ToLower()];
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
            line = IdRegex.Replace(line, string.Empty);
            var match = Regex.Match(line);

            TimeStamp = DateTime.Parse(match.Groups[1].Value);
            Source = match.Groups[2].Value;
            Target = match.Groups[3].Value;
            Ability = match.Groups[4].Value;

            if (match.Groups[5].Value.Contains(":"))
            {
                EventType = match.Groups[5].Value.Split(':')[0];
                EventDetail = match.Groups[5].Value.Split(':')[1].Trim();
            }
            else
            {
                EventType = match.Groups[5].Value;
                EventDetail = string.Empty;
            }

            CritValue = match.Groups[6].Value.Contains("*");
            var rawValue = match.Groups[6].Value.Replace("*", string.Empty).Split(' ');
            Value = rawValue[0].Length > 0 ? int.Parse(rawValue[0]) : 0;
            ValueType = rawValue.Length > 1 ? rawValue[1] : string.Empty;
            Threat = match.Groups[8].Value.Length > 0 ? int.Parse(match.Groups[8].Value) : 0;
        }
    }
}