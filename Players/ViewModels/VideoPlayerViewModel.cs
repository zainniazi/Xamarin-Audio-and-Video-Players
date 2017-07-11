using System;
using Players.Helpers;

namespace Players.ViewModels
{
	public class VideoPlayerViewModel : BaseNavigationViewModel
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
