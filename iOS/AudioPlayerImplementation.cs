using System;
using AVFoundation;
using Foundation;
using Players.Helpers;
using Players.iOS;
using Players.iOS.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(AudioPlayerImplementation))]
namespace Players.iOS
{
    public class AudioPlayerImplementation : IAudioPlayer
    {
        #region ParentEvents
        public event EventHandler HasEndedEvent;
        public event EventHandler<double> HasStarted;
        public event EventHandler<double> SliderValueChange;
        #endregion

        #region LocalVariables
        Constants.ContentType _contentType;
        bool _hasEnded = true;
        double _totalLength;

        AVPlayer _player;
        NSUrl _soundUrl;
        NSObject _timeObserver;
        #endregion

        public AudioPlayerImplementation()
        {
            _player = new AVPlayer(){
                ActionAtItemEnd = AVPlayerActionAtItemEnd.None
            };
        }

        public void Stop()
        {
            _player?.Stop();
        }

        public void Play()
        {
            if (!_hasEnded)
            {
                _player.Play();
            }
            else
            {
                // Any existing sound?
                 Stop();
                _hasEnded = false;
                _totalLength = _player.CurrentItem.Asset.Duration.Seconds;
                HasStarted?.Invoke(this, _totalLength);
                _player.Play();
                // Observe Time of Audio
                StartTimeObserver();
            }
        }

        void StartTimeObserver()
        {
            _timeObserver = _player
                .AddPeriodicTimeObserver(CoreMedia.CMTime.FromSeconds(1.0 / 60.0, Constants.NSEC_PER_SEC), null, (obj) =>
                         {
                             if (_player != null)
                             {
                                 SliderValueChange?.Invoke(new object(), obj.Seconds);
                                 if (obj.Seconds >= _totalLength)
                                 {
                                     _hasEnded = true;
                                     HasEndedEvent?.Invoke(this, new EventArgs());
                                     EndTimeObserver();
                                 }
                             }
                             else
                             {
                                 EndTimeObserver();

                             }
                         });
        }

        void EndTimeObserver()
        {
            _timeObserver?.Dispose();
        }

        public void Pause()
        {
            _player?.Pause();
        }

        public void GoToTime(double sec)
        {
            if (_player != null)
            {
                _player.SeekAsync(CoreMedia.CMTime.FromSeconds(sec, Constants.NSEC_PER_SEC));
            }
            else
            {
                Play();
            }
        }

        public void Dispose()
        {
            _player?.Dispose();
        }

        public void SetData(Constants.ContentType type, string source)
        {
            _contentType = type;
            _soundUrl = _contentType == Constants.ContentType.Remote ? new NSUrl(source) : new NSUrl(source, false);
            _player.ReplaceCurrentItemWithPlayerItem(AVPlayerItem.FromUrl(_soundUrl));
        }
    }
}
