﻿using System;
using AVFoundation;
using Players.Helpers;

namespace Players.iOS.Helpers
{
	public static class Extensions
	{
		public static void Stop(this AVPlayer player)
		{
			player?.Pause();
			player?.Seek(CoreMedia.CMTime.FromSeconds(0.0, Constants.NSEC_PER_SEC));
		}
	}
}
