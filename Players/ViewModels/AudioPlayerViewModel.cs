using Players.Helpers;

namespace Players.ViewModels
{
    public class AudioPlayerViewModel : BaseNavigationViewModel
    {
        public string Source
        {
            get;
            set;
        }
        public Constants.ContentType ContentType
        {
            get;
            set;
        }
    }
}