namespace SwtorCaster
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using static System.Environment;

    public class LogLine
    {
        private static readonly Dictionary<string, string> Files;
        public static readonly string Missing = Path.Combine(CurrentDirectory, "Images", "missing.png");

        static LogLine()
        {
            Files = Directory.GetFiles(Path.Combine(CurrentDirectory, "Images"))
                             .ToDictionary(k => Path.GetFileNameWithoutExtension(k), v => v);
        }

        public DateTime TimeStamp { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Ability { get; set; }
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
                    
                }

                return Missing;
            }
        }

        static readonly Regex Regex = new Regex(@"\[(.*)\] \[(.*)\] \[(.*)\] \[(.*)\] \[(.*)\] \((.*)\)[.<]*([!>]*)[\s<]*(\d*)?[>]*", RegexOptions.Compiled);
        static readonly Regex IdRegex = new Regex(@"\s*\{\d*}\s*", RegexOptions.Compiled);

        public LogLine(string line)
        {
            line = IdRegex.Replace(line, string.Empty);
            Match match = Regex.Match(line);

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
            string[] rawValue = match.Groups[6].Value.Replace("*", "").Split(' ');
            Value = rawValue[0].Length > 0 ? int.Parse(rawValue[0]) : 0;
            ValueType = rawValue.Length > 1 ? rawValue[1] : string.Empty;
            Threat = match.Groups[8].Value.Length > 0 ? int.Parse(match.Groups[8].Value) : 0;
        }
    }
}