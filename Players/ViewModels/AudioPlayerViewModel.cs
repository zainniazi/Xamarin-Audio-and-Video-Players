using System;
using Players.Helpers;
using Xamarin.Forms;

namespace Players.ViewModels
{
    public class AudioPlayerViewModel : BaseNavigationViewModel
    {
        readonly IAudioPlayer player;
        enum PlayBackStatus
        {
            Play, Pause, Stop
        }
        public AudioPlayerViewModel()
        {
            SliderMaxValue = 1;
            player = DependencyService.Get<IAudioPlayer>();
            if (player != null)
            {
                player.SliderValueChange += (sender, e) =>
                {
                    CurrentPosition = TimeSpan.FromSeconds(Math.Ceiling(e)).ToString("mm\\:ss");
                    if (!IsSliderPressed)
                        SliderCurrentValue = e;
                };
                player.SoundLength += (sender, e) =>
                {
                    IsEnabled = true;
					SliderMaxValue = Math.Ceiling(e);
					Length = TimeSpan.FromSeconds(SliderMaxValue).ToString("mm\\:ss");
                };
                player.HasEndedEvent += (sender, e) =>
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
                    player.GoToTime(_SliderCurrentValue);
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
        bool IsPlaying;


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
                player?.Pause();
            }
            else
            {
                PlayPauseIcon = PlayBackStatus.Pause.ToString();
                IsPlaying = true;
                player?.Play();
            }

        }

        Command _StopCommand;
        public Command StopCommand => _StopCommand ??
                                        (_StopCommand = new Command(ExecuteStopCommand));

        void ExecuteStopCommand()
        {
            player?.Stop();
            IsEnabled = false;
        }
        #endregion
    }
}
