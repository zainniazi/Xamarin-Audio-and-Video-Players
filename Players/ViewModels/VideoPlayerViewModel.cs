using System;
using Players.Helpers;
using Xamarin.Forms;

namespace Players.ViewModels
{
    public class VideoPlayerViewModel : BaseNavigationViewModel
    {
        bool IsPlaying;
        VideoPlayer videoPlayer;
        enum PlayBackStatus
        {
            Play, Pause, Stop
        }
        public VideoPlayerViewModel(VideoPlayer videoPlayer)
        {
            this.videoPlayer = videoPlayer;
            SliderMaxValue = 1;
            if (videoPlayer != null)
            {
                videoPlayer.SliderValueChange += (sender, e) =>
                {
                    CurrentPosition = TimeSpan.FromSeconds(Math.Ceiling(e)).ToString("mm\\:ss");
                    if (!IsSliderPressed)
                        SliderCurrentValue = e;
                };
                videoPlayer.SoundLength += (sender, e) =>
                {
                    IsEnabled = true;
                    SliderMaxValue = Math.Ceiling(e);
                    Length = TimeSpan.FromSeconds(SliderMaxValue).ToString("mm\\:ss");
                };
                videoPlayer.HasEndedEvent += (sender, e) =>
                {
                    IsPlaying = false;
                    IsEnabled = false;
                    SliderCurrentValue = 0.0;
                    CurrentPosition = TimeSpan.FromSeconds(0.0).ToString("mm\\:ss");
                    PlayPauseIcon = PlayBackStatus.Play.ToString();
                };
            }
        }

        public bool IsSliderPressed { get; set; }
        bool _IsEnabled;
        public bool IsEnabled
        {

            get { return _IsEnabled; }
            set
            {
                _IsEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }
        double _SliderMaxValue;
        public double SliderMaxValue
        {
            get { return _SliderMaxValue; }
            set
            {
                _SliderMaxValue = value;
                OnPropertyChanged("SliderMaxValue");
            }
        }
        double _SliderCurrentValue;
        public double SliderCurrentValue
        {
            get
            {
                return _SliderCurrentValue;
            }
            set
            {
                _SliderCurrentValue = value;
                if (IsSliderPressed)
                {
                    videoPlayer.GoToTime?.Invoke(this, _SliderCurrentValue);
                }
                else
                {
                    OnPropertyChanged("SliderCurrentValue");
                }
            }
        }

        string _PlayPauseIcon = PlayBackStatus.Play.ToString();
        public string PlayPauseIcon
        {
            get { return _PlayPauseIcon; }
            set
            {
                _PlayPauseIcon = value;
                OnPropertyChanged("PlayPauseIcon");
            }
        }
        public string StopText = PlayBackStatus.Stop.ToString();

        string _CurrentPosition = "00:00";
        public string CurrentPosition
        {
            get { return _CurrentPosition; }
            set
            {
                _CurrentPosition = value;
                OnPropertyChanged("CurrentPosition");
            }
        }

        string _Length = "00:00";
        public string Length
        {
            get { return _Length; }
            set
            {
                _Length = value;
                OnPropertyChanged("Length");
            }
        }

        #region Commands
        Command _PlayPauseCommand;
        public Command PlayPauseCommand => _PlayPauseCommand ??
                                        (_PlayPauseCommand = new Command(ExecutePlayPauseCommand));

        void ExecutePlayPauseCommand()
        {
            if (IsPlaying)
            {
                PlayPauseIcon = PlayBackStatus.Play.ToString();
                IsPlaying = false;
                videoPlayer?.Pause?.Invoke(new object(), new EventArgs());
            }
            else
            {
                PlayPauseIcon = PlayBackStatus.Pause.ToString();
                IsPlaying = true;
                videoPlayer?.Play?.Invoke(new object(), new EventArgs());
            }
        }

        Command _StopCommand;
        public Command StopCommand => _StopCommand ?? (_StopCommand = new Command(ExecuteStopCommand));
        void ExecuteStopCommand()
        {
            videoPlayer?.Stop?.Invoke(new object(), new EventArgs());
            IsEnabled = false;
        }
        #endregion
    }
}
