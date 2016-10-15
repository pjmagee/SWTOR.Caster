using System;
using System.Collections.Generic;

namespace SwtorCaster.Core.Services.Audio
{
    public interface IAudioService
    {
        void Play(string audioFile);

        void Stop();

        IEnumerable<KeyValuePair<string, Guid>> GetAudioDevices();
    }
}