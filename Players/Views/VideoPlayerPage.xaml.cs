using System;
using System.Collections.Generic;
using Players.Helpers;
using Players.ViewModels;
using Xamarin.Forms;

namespace Players.Views
{
    public partial class VideoPlayerPage : ContentPage
	{
        protected VideoPlayerViewModel ViewModel => BindingContext as VideoPlayerViewModel;
        public VideoPlayerPage()
        {
			InitializeComponent();
			BindingContext = new VideoPlayerViewModel()
			{
				ContentType = Constants.ContentType.Remote,
				Source = "http://www.androidbegin.com/tutorial/AndroidCommercial.3gp"
			};
        }
    }
}
