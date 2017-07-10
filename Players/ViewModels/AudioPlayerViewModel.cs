using Players.Helpers;
using Players.ViewModels;
namespace Players.iOS
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