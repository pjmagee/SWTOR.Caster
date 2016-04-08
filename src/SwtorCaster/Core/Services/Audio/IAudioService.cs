namespace SwtorCaster.Core.Services.Audio
{
    public interface IAudioService
    {
        void Play(string audioFile, int volume = 100);
        void Stop();
    }
}