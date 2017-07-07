using System;
using AVFoundation;
using Foundation;
using Players.Helpers;
using Players.iOS;

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
        Constants.ContentType ContentType;
        bool HasEnded;
        double TotalLength;

        AVPlayer _player;
        NSUrl soundUrl;
        NSObject TimeObserver;
        #endregion

        public void Stop()
        {
            if (_player != null)
            {
                _player.Stop();
            }
        }

        public void Play()
        {
            if (ContentType == Constants.ContentType.Remote)
                soundUrl = new NSUrl("https://swaong.azurewebsites.net/Uploads/messagedd58c21a-d4af-460d-9ae5-8e966b017a54-20170705132008aac-msg..wav");
            else
                soundUrl = new NSUrl("A-Team.wav", false);
            if (!HasEnded && _player != null)
            {
                _player.Play();
            }
            else
            {
                HasEnded = false;
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
                _player = AVPlayer.FromUrl(soundUrl);
                TotalLength = _player.CurrentItem.Asset.Duration.Seconds;
                SoundLength?.Invoke(this, TotalLength);
                _player.Play();
                // Observe Time of Audio
                StartTimeObserver();
                _player.ActionAtItemEnd = AVPlayerActionAtItemEnd.None;
            }
        }

        void StartTimeObserver()
        {
            TimeObserver = _player
                .AddPeriodicTimeObserver(CoreMedia.CMTime.FromSeconds(1.0 / 60.0, Constants.NSEC_PER_SEC), null, (obj) =>
                         {
                             if (_player != null)
                             {
                                 SliderValueChange?.Invoke(new object(), obj.Seconds);
                                 if (obj.Seconds >= TotalLength)
                                 {
                                     HasEnded = true;
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
            TimeObserver?.Dispose();
        }

        public void Pause()
        {
            if (_player != null)
            {
                _player.Pause();
            }
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
            if (_player != null)
            {
                _player.Dispose();
            }
        }

        public void SetContentType(Constants.ContentType type)
        {
            ContentType = type;
        }
    }

    public static class Extensions
    {
        public static void Stop(this AVPlayer _player)
        {
            _player?.Pause();
            _player?.Seek(CoreMedia.CMTime.FromSeconds(0.0, Constants.NSEC_PER_SEC));
        }
    }

}
