using System;
namespace Players.Helpers
{
    public interface IAudioPlayer : IDisposable
    {
        void SetData(Constants.ContentType type, string source);
		void Stop();
		void Play();
		void Pause();
        void GoToTime(double sec);
        event EventHandler<double> SliderValueChange;
		event EventHandler<double> HasStarted;
		event EventHandler HasEndedEvent;
    }
}
