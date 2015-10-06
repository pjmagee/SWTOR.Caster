namespace SwtorCaster
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static System.Environment;

    public class CombatLogParser
    {
        private readonly string _path;
        // private readonly Regex _regex = new Regex(@"\[(?<date>.*?)\] \[(?<source>.*?)\] \[(?<target>.*?)\] \[(?<ability>.*?)\] \[(?<effect>.*?)\] \((?<resource>.*?)\)", RegexOptions.Compiled | RegexOptions.Multiline);

        private CancellationTokenSource _tokenSource;

        public event EventHandler<LogLine> ItemAdded;

        public CombatLogParser(string path)
        {
            _path = path;
        }

        public void Start()
        {
            try
            {
                var fileInfos = new DirectoryInfo(_path).GetFiles("*.txt", SearchOption.TopDirectoryOnly);
                var fileInfo = fileInfos.OrderByDescending(x => x.LastWriteTime).FirstOrDefault();
                Open(fileInfo.FullName);
            }
            catch(Exception e)
            {
                File.AppendAllText(Path.Combine(CurrentDirectory, "log.txt"), $"Error starting: {e.Message} {NewLine}");
            }
        }

        private void Open(string file)
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(() => Read(file), _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        public void Read(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                using (var reader = new StreamReader(fs))
                {
                    reader.ReadToEnd();

                    while (true)
                    {
                        string value = reader.ReadLine();
                        if (value == null) continue;

                        try
                        {
                            ItemAdded?.Invoke(this, new LogLine(value));
                        }
                        catch (Exception e)
                        {
                            if (App.EnableLog)
                            {
                                File.AppendAllText(Path.Combine(CurrentDirectory, "log.txt"), $"Error adding item: {e.Message} {NewLine}");
                            }
                        }
                    }
                }
            }
        }
    }
}