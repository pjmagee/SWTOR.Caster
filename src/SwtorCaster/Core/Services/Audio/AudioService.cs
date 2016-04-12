namespace SwtorCaster.Core.Services.Audio
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NAudio.Wave;

    public class AudioService : IAudioService
    {
        private WaveOut waveOut;
        private ManualResetEvent _event;

        public async void Play(string audioFile, int volume)
        {
            waveOut?.Stop();

            await Task.Run(() =>
            {
                try
                {
                    using (var audioFileReader = new AudioFileReader(audioFile))
                    {
                        audioFileReader.Volume = volume * 0.01f;

                        using (waveOut = new WaveOut())
                        {
                            waveOut.Init(audioFileReader);

                            using (_event = new ManualResetEvent(false))
                            {
                                waveOut.Play();
                                waveOut.PlaybackStopped += (sender, args) => _event.Set();
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
            waveOut?.Stop();
        }
    }
}