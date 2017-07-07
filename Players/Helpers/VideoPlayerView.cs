using System;
using Xamarin.Forms;

namespace Players.Helpers
{
    public class VideoPlayerView : ContentView
    {
        public VideoPlayerView()
        {
        }
        //void SetContentType(Constants.ContentType type);
        //void Stop();
        //void Play();
        //void Pause();
        //void GoToTime(double sec);
        event EventHandler<double> SliderValueChange;
        event EventHandler<double> SoundLength;
        event EventHandler HasEndedEvent;
    }
}
