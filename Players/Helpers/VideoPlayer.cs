using System;

using Xamarin.Forms;

namespace Players.Helpers
{
    public class VideoPlayer : ContentView
    {
        public Constants.ContentType ContentType { get; set; }
        public string Source { get; set; }
        public EventHandler Play;
        public EventHandler Pause;
        public EventHandler Stop;
        public EventHandler<double> SliderValueChange;
        public EventHandler<double> SoundLength;
        public EventHandler HasEndedEvent;
        public EventHandler<double> GoToTime;
        public void SetData(Constants.ContentType contentType, string source)
        {
            ContentType = contentType;
            Source = source;
        }
    }
}

