using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Players.Views
{
    public partial class PlayersPage : ContentPage
    {
        public PlayersPage()
        {
            InitializeComponent();
        }

        void Audio_Clicked(object sender, System.EventArgs e)
        {
            Navigation?.PushAsync(new AudioPlayerPage());
        }
		void Video_Clicked(object sender, System.EventArgs e)
		{
            Navigation?.PushAsync(new VideoPlayerPage());
		}
    }
}
