namespace SwtorCaster.Core.Services.Audio
{
    using System.Threading;
    using System.Threading.Tasks;
    using NAudio.Wave;

    public class AudioService : IAudioService
    {
        private CancellationTokenSource _tokenSource;
        private ManualResetEventSlim _resetEventSlim;

        public async void Play(string audioFile, int volume)
        {
            _tokenSource = new CancellationTokenSource();
            _resetEventSlim = new ManualResetEventSlim();

            await Task.Run(() =>
            {
                using (var reader = new AudioFileReader(audioFile))
                {
                    reader.Volume = volume * 0.01f;

                    using (var soundOut = new DirectSoundOut())
                    {
                        soundOut.Init(reader);
                        soundOut.Play();
                        soundOut.PlaybackStopped += (sender, args) => _resetEventSlim.Set();
                        _resetEventSlim.Wait();
                    }
                }

            }, _tokenSource.Token);
        }

        public void Stop()
        {
            _resetEventSlim?.Set();
            _tokenSource?.Cancel();
        }
    }
}