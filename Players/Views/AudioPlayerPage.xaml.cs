using System;
using System.Collections.Generic;
using Players.Helpers;
using Players.iOS;
using Xamarin.Forms;

namespace Players.Views
{
    public partial class AudioPlayerPage : ContentPage
    {
        protected AudioPlayerViewModel ViewModel => BindingContext as AudioPlayerViewModel;
        public AudioPlayerPage()
        {
            InitializeComponent();
            BindingContext = new AudioPlayerViewModel(){
                ContentType = Constants.ContentType.Remote,
                Source = "https://swaong.azurewebsites.net/Uploads/messagedd58c21a-d4af-460d-9ae5-8e966b017a54-20170705132008aac-msg..wav"
            };
        }

        protected override bool OnBackButtonPressed()
        {
            AudioPlayer.Dispose();
            return base.OnBackButtonPressed();
        }
    }
}
