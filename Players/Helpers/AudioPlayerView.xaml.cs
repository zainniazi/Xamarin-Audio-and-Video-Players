using System;
using System.Collections.Generic;
using Players.ViewModels;
using Xamarin.Forms;

namespace Players.Helpers
{
	public partial class AudioPlayerView : ContentView
	{
		protected AudioPlayerViewModel ViewModel => BindingContext as AudioPlayerViewModel;
		public AudioPlayerView()
		{
			InitializeComponent();
			BindingContext = new AudioPlayerViewModel();
		}
	}
}