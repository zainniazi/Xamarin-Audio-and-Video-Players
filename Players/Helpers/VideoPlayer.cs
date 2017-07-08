using System;

using Xamarin.Forms;

namespace Players.Helpers
{
    public class VideoPlayer : View
    {
		public EventHandler Play;
		public EventHandler Pause;
		public EventHandler Stop;
        public EventHandler<double> SliderValueChange;
        public EventHandler<double> SoundLength;
        public EventHandler HasEndedEvent;
		public EventHandler<double> GoToTime;
    }
}

