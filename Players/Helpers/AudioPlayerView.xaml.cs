using System;
using System.Collections.Generic;
using Players.ViewModels;
using Xamarin.Forms;

namespace Players.Helpers
{
    public partial class AudioPlayerView : ContentView, IDisposable
    {
        public AudioPlayerView()
        {
            InitializeComponent();
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
                player.HasStarted += (sender, e) =>
                {
                    SliderMaxValue = Math.Ceiling(e);
					Length = TimeSpan.FromSeconds(SliderMaxValue).ToString("mm\\:ss");
					IsSliderEnabled = true;
                };
                player.HasEndedEvent += (sender, e) =>
                {
                    IsPlaying = false;
                    IsSliderEnabled = false;
                    SliderCurrentValue = 0.0;
                    CurrentPosition = TimeSpan.FromSeconds(0.0).ToString("mm\\:ss");
                    PlayPauseIcon = PlayBackStatus.Play.ToString();
                };
            }
            Content.BindingContext = this;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            player.SetData(ContentType, Source);
        }

        #region Public 

        public static readonly BindableProperty ContentTypeProperty =
            BindableProperty.Create("ContentType", typeof(Constants.ContentType), typeof(AudioPlayerView), Constants.ContentType.Local);

        public Constants.ContentType ContentType
        {
            get => (Constants.ContentType)GetValue(ContentTypeProperty);
            set => SetValue(ContentTypeProperty, value);
        }

        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create("Source", typeof(string), typeof(AudioPlayerView), string.Empty);

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }


        readonly IAudioPlayer player;
        enum PlayBackStatus
        {
            Play, Pause, Stop
        }


        public bool IsSliderPressed { get; set; }
        bool _IsSliderEnabled;
        public bool IsSliderEnabled
        {

            get { return _IsSliderEnabled; }
            set
            {
                _IsSliderEnabled = value;
                OnPropertyChanged("IsSliderEnabled");
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
            IsSliderEnabled = false;
        }
        #endregion

        public void Dispose()
        {
            player?.Dispose();
        }

        #endregion
    }
}