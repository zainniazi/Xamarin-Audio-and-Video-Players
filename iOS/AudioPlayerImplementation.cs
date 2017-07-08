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
        public event EventHandler<double> SoundLength;
        public event EventHandler<double> SliderValueChange;
        #endregion

        #region LocalVariables
        Constants.ContentType _contentType;
        bool _hasEnded;
        double _totalLength;

        AVPlayer _player;
        NSUrl _soundUrl;
        NSObject _timeObserver;
        #endregion

        public void Stop()
        {
            _player?.Stop();
        }

        public void Play()
        {
            _soundUrl = _contentType == Constants.ContentType.Remote ? new NSUrl("https://swaong.azurewebsites.net/Uploads/messagedd58c21a-d4af-460d-9ae5-8e966b017a54-20170705132008aac-msg..wav") : new NSUrl("A-Team.wav", false);
            if (!_hasEnded && _player != null)
            {
                _player.Play();
            }
            else
            {
                _hasEnded = false;
                // Any existing sound?
                if (_player != null)
                {
                    //Stop and dispose of any background music
                    _player.Stop();
                    EndTimeObserver();
                }
                else
                {
                    _player = new AVPlayer();
                }
                _player = AVPlayer.FromUrl(_soundUrl);
                _totalLength = _player.CurrentItem.Asset.Duration.Seconds;
                SoundLength?.Invoke(this, _totalLength);
                _player.Play();
                // Observe Time of Audio
                StartTimeObserver();
                _player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;
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

        public void SetContentType(Constants.ContentType type)
        {
            _contentType = type;
        }
    }
}
