using GoogleMobileAds.Api;
using UnityEngine;

namespace Gamr.Monetization.Ads
{
	public class AdmobInterstitialAd
	{
		private readonly string _unitId;

		private InterstitialAd _ad;

		public AdmobInterstitialAd(string unitId)
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
			InterstitialAd.Load(_unitId, req,
				(ad, error) =>
				{
					if (error != null || ad == null)
					{
						Debug.LogError("InterstitialAd loading failed with error: " + error);
						return;
					}
					Debug.Log("InterstitialAd loaded with response: " + ad.GetResponseInfo());

					_ad = ad;
					RegisterReloadHandler(_ad);

                });
		}

		public bool Show()
		{
			Debug.Log("Showing interstitial ad.");
			
			if (_ad != null && _ad.CanShowAd())
			{
				_ad.Show();
				return true;
			}
			else
			{
				return false;
			}
        }
        private void RegisterReloadHandler(InterstitialAd interstitialAd)
        {
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad full screen content closed.");

                Load();
            };
            interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Interstitial ad failed to open full screen content " +
                               "with error : " + error);

                Load();
            };
        }
    }
}
