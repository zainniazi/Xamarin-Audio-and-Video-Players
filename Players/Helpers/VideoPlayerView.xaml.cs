using System;
using System.Collections.Generic;
using Players.ViewModels;
using Xamarin.Forms;

namespace Players.Helpers
{
    public partial class VideoPlayerView : ContentView
	{
        protected VideoPlayerViewModel ViewModel => BindingContext as VideoPlayerViewModel;
        public VideoPlayerView()
        {
            InitializeComponent();
            BindingContext = new VideoPlayerViewModel(VPlayer);
        }
    }
}
