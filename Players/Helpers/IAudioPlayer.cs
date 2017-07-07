using System;
namespace Players.Helpers
{
    public interface IAudioPlayer : IDisposable
    {
        void SetContentType(Constants.ContentType type);
		void Stop();
		void Play();
		void Pause();
        void GoToTime(double sec);
        event EventHandler<double> SliderValueChange;
		event EventHandler<double> SoundLength;
		event EventHandler HasEndedEvent;
    }
}
