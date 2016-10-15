namespace SwtorCaster.Core.Services.Audio
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NAudio.Wave;
    using System.Linq;
    using Settings;

    public class AudioService : IAudioService
    {
        private DirectSoundOut dso;
        private ManualResetEvent eventWaiter;
        private ISettingsService settingsService;

        public AudioService(ISettingsService settingsService)
        {         
            this.settingsService = settingsService;
        }

        public IEnumerable<KeyValuePair<string, Guid>> GetAudioDevices()
        {
            return DirectSoundOut.Devices.Select(device => new KeyValuePair<string, Guid>(device.Description, device.Guid));
        }

        public async void Play(string audioFile)
        {
            Stop();
            await Start(audioFile);
        }

        public void Stop()
        {
            dso?.Stop();
        }

        private async Task Start(string audioFile)
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var audioFileReader = new AudioFileReader(audioFile))
                    {
                        audioFileReader.Volume = settingsService.Settings.Volume * 0.01f;

                        using (dso = new DirectSoundOut(settingsService.Settings.AudioDeviceId))
                        {
                            dso.Init(audioFileReader);

                            using (eventWaiter = new ManualResetEvent(false))
                            {
                                dso.Play();
                                dso.PlaybackStopped += (sender, args) => eventWaiter.Set();
                                eventWaiter.WaitOne();
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
            });
        }
    }
}