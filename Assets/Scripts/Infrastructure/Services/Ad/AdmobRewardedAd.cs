using GoogleMobileAds.Api;
using System;
using UnityEngine;

namespace Gamr.Monetization.Ads
{
	public class AdmobRewardedAd
	{
		private readonly string _unitId;

		private RewardedAd _ad;

		public AdmobRewardedAd(string unitId)
		{
			_unitId = unitId;
		}

		public void Load()
		{
			if (_ad != null)
			{
				_ad.Destroy();
				_ad = null;
			}

			var req = new AdRequest();
			RewardedAd.Load(_unitId, req,
				(ad, error) =>
				{
					if (error != null || ad == null)
					{
						Debug.LogError("InterstitialAd loading failed with error: " + error);
						return;
					}
					Debug.Log("InterstitialAd loaded with response: " + ad.GetResponseInfo());

					_ad = ad;
				});
		}

		public bool Show(Action<Reward> callback)
		{
			Debug.Log("Showing interstitial ad.");

			if (_ad != null && _ad.CanShowAd())
			{
				_ad.Show(callback);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
