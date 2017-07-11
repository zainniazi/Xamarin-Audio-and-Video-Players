using System;
using System.Threading;
using AVFoundation;
using CoreGraphics;
using Foundation;
using Players.Helpers;
using Players.iOS;
using Players.iOS.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(VideoPlayer), typeof(VideoPlayerImplementation))]
namespace Players.iOS
{
    public class VideoPlayerImplementation : ViewRenderer
    {
        //globally declare variables
        AVAsset _asset;
        AVPlayerItem _playerItem;
        AVPlayer _player;
        AVPlayerLayer _playerLayer;
        NSObject _timeObserver;
        double _totalLength;
        Constants.ContentType _contentType;
        Thread videoLengthThread;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement is VideoPlayer parentPlayer)
            {
                _player = new AVPlayer();
                _playerLayer = AVPlayerLayer.FromPlayer(_player);

                _contentType = parentPlayer.ContentType;
                var url = _contentType == Constants.ContentType.Remote ? new NSUrl(parentPlayer.Source) : new NSUrl(parentPlayer.Source, false);
                _asset = AVAsset.FromUrl(url);
                _playerItem = new AVPlayerItem(_asset);
                _player.ReplaceCurrentItemWithPlayerItem(_playerItem);

                videoLengthThread = new Thread(new ThreadStart(() =>
                {
                    _totalLength = _player.CurrentItem.Asset.Duration.Seconds;
                    InvokeOnMainThread(() =>
                    {
                        parentPlayer.SoundLength?.Invoke(this, _totalLength);
                    });
                }));
                videoLengthThread.Start();

                parentPlayer.Play += (sender, innere) =>
                {
                    if (videoLengthThread.IsAlive)
                    {
                        videoLengthThread.Abort();
                        _totalLength = _player.CurrentItem.Asset.Duration.Seconds;
                        parentPlayer.SoundLength?.Invoke(this, _totalLength);
                    }
                    StartTimeObserver(parentPlayer);
                    _player.Play();
                };

                parentPlayer.Pause += (sender, innere) =>
                {
                    _player.Pause();
                };

                parentPlayer.Stop += (sender, innere) =>
                {
                    EndTimeObserver();
                    _player.Stop();
                };

                parentPlayer.GoToTime += (sender, sec) =>
                {
                    _player.SeekAsync(CoreMedia.CMTime.FromSeconds(sec, Constants.NSEC_PER_SEC));
                };
            }
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _playerLayer.Frame = new CGRect(NativeView.Frame.X, NativeView.Frame.Y,
                                            NativeView.Frame.Width, NativeView.Frame.Height);
            NativeView.Layer.AddSublayer(_playerLayer);
        }

        void StartTimeObserver(VideoPlayer parentPlayer)
        {
            _timeObserver = _player
                .AddPeriodicTimeObserver(CoreMedia.CMTime.FromSeconds(1.0 / 60.0, Constants.NSEC_PER_SEC), null, (obj) =>
                         {
                             if (_player != null)
                             {
                                 parentPlayer?.SliderValueChange?.Invoke(this, obj.Seconds);
                                 if (obj.Seconds >= _totalLength)
                                 {
                                     _player.SeekAsync(CoreMedia.CMTime.FromSeconds(0, Constants.NSEC_PER_SEC));
                                     parentPlayer?.HasEndedEvent?.Invoke(this, new EventArgs());
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _player?.Dispose();
            _playerLayer?.Dispose();
            _playerItem?.Dispose();
            _asset?.Dispose();
            _timeObserver?.Dispose();
        }
    }
}
