using GoogleMobileAds.Api;
using System;

namespace Game.Monetization.Ads
{
	public interface IAdsService
	{
		void Initialize();

		void ShowInterstitial();
		void ShowRewarded(Action<Reward> callback);
	}
}
