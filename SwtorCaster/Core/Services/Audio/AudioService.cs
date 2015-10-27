namespace SwtorCaster.Core.Services.Audio
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NAudio.Wave;
    using Logging;

    public class AudioService : IAudioService
    {
        private WaveOut _waveOut;
        private ManualResetEvent _event;

        private readonly ILoggerService _loggerService;

        public AudioService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
        
        public async void Play(string audioFile, int volume)
        {
            _waveOut?.Stop();

            await Task.Run(() =>
            {
                try
                {
                    using (var audioFileReader = new AudioFileReader(audioFile))
                    {
                        audioFileReader.Volume = volume * 0.01f;

                        using (_waveOut = new WaveOut())
                        {
                            _waveOut.Init(audioFileReader);

                            using (_event = new ManualResetEvent(false))
                            {
                                _waveOut.Play();
                                _waveOut.PlaybackStopped += (sender, args) => _event.Set();
                                _event.WaitOne();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    
                }
            });
        }

        public void Stop()
        {
            _waveOut?.Stop();
        }
    }
}