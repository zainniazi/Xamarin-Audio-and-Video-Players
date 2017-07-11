using System;
using System.Collections.Generic;
using Players.ViewModels;
using Xamarin.Forms;

namespace Players.Helpers
{
    public partial class VideoPlayerView : ContentView
    {
        public VideoPlayerView()
        {
            InitializeComponent();
            SliderMaxValue = 1;
            VPlayer.SliderValueChange += (sender, e) =>
            {
                CurrentPosition = TimeSpan.FromSeconds(Math.Ceiling(e)).ToString("mm\\:ss");
                if (!IsSliderPressed)
                    SliderCurrentValue = e;
            };
            VPlayer.SoundLength += (sender, e) =>
            {
                IsSliderEnabled = true;
                SliderMaxValue = Math.Ceiling(e);
                Length = TimeSpan.FromSeconds(SliderMaxValue).ToString("mm\\:ss");
            };
            VPlayer.HasEndedEvent += (sender, e) =>
            {
                IsPlaying = false;
                IsSliderEnabled = false;
                SliderCurrentValue = 0.0;
                CurrentPosition = TimeSpan.FromSeconds(0.0).ToString("mm\\:ss");
                PlayPauseIcon = PlayBackStatus.Play.ToString();
            };
            Content.BindingContext = this;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            VPlayer.SetData(ContentType, Source);
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

        bool IsPlaying;
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
                    VPlayer?.GoToTime?.Invoke(this, _SliderCurrentValue);
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
                VPlayer?.Pause?.Invoke(new object(), new EventArgs());
            }
            else
            {
                PlayPauseIcon = PlayBackStatus.Pause.ToString();
                IsPlaying = true;
                VPlayer?.Play?.Invoke(new object(), new EventArgs());
            }
        }

        Command _StopCommand;
        public Command StopCommand => _StopCommand ?? (_StopCommand = new Command(ExecuteStopCommand));
        void ExecuteStopCommand()
        {
            VPlayer?.Stop?.Invoke(new object(), new EventArgs());
            IsSliderEnabled = false;
        }

        #endregion
        #endregion
    }
}
