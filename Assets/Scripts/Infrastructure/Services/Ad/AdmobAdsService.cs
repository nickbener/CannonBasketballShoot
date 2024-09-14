using Gamr.Monetization.Ads;
using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Monetization.Ads
{
	public class AdmobAdsService : IAdsService
	{
		//TEST
		private const string INTERSTITIAL_CHEEP_UNIT_ID = "ca-app-pub-3940256099942544/1033173712";
		private const string INTERSTITIAL_NORMAL_UNIT_ID = "ca-app-pub-3940256099942544/1033173712";
		private const string INTERSTITIAL_EXPENSIVE_UNIT_ID = "ca-app-pub-3940256099942544/1033173712";

		private const string REWARDED_CHEEP_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";
		private const string REWARDED_NORMAL_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";
		private const string REWARDED_EXPENSIVE_UNIT_ID = "ca-app-pub-3940256099942544/5224354917";
		//ADMOB
		//private const string INTERSTITIAL_CHEEP_UNIT_ID = "ca-app-pub-8340576279106634/4053800891";
		//private const string INTERSTITIAL_NORMAL_UNIT_ID = "ca-app-pub-8340576279106634/7646966519";
		//private const string INTERSTITIAL_EXPENSIVE_UNIT_ID = "ca-app-pub-8340576279106634/3324578125";

		//private const string REWARDED_CHEEP_UNIT_ID = "ca-app-pub-8340576279106634/7159881359";
		//private const string REWARDED_NORMAL_UNIT_ID = "ca-app-pub-8340576279106634/9043808665";
		//private const string REWARDED_EXPENSIVE_UNIT_ID = "ca-app-pub-8340576279106634/9769162003";

		private AdmobInterstitialAd _cheepInterstitial;
		private AdmobInterstitialAd _normalInterstitial;
		private AdmobInterstitialAd _expensiveInterstitial;

		private AdmobRewardedAd _cheepRewarded;
		private AdmobRewardedAd _normalRewarded;
		private AdmobRewardedAd _expensiveRewarded;

		//private static bool _isInitialized;

		//private void Start()
		//{
		//	if (_isInitialized)
		//		return;
		//	_isInitialized = true;
		//	Instantiate();
		//}

		//private void Instantiate()
		//{
		//    _cheepInterstitial = new AdmobInterstitialAd(INTERSTITIAL_CHEEP_UNIT_ID);
		//    _normalInterstitial = new AdmobInterstitialAd(INTERSTITIAL_NORMAL_UNIT_ID);
		//    _expensiveInterstitial = new AdmobInterstitialAd(INTERSTITIAL_EXPENSIVE_UNIT_ID);

		//    _cheepRewarded = new AdmobRewardedAd(REWARDED_CHEEP_UNIT_ID);
		//    _normalRewarded = new AdmobRewardedAd(REWARDED_NORMAL_UNIT_ID);
		//    _expensiveRewarded = new AdmobRewardedAd(REWARDED_EXPENSIVE_UNIT_ID);
		//}

		public AdmobAdsService()
		{
			_cheepInterstitial = new AdmobInterstitialAd(INTERSTITIAL_CHEEP_UNIT_ID);
			_normalInterstitial = new AdmobInterstitialAd(INTERSTITIAL_NORMAL_UNIT_ID);
			_expensiveInterstitial = new AdmobInterstitialAd(INTERSTITIAL_EXPENSIVE_UNIT_ID);

			_cheepRewarded = new AdmobRewardedAd(REWARDED_CHEEP_UNIT_ID);
			_normalRewarded = new AdmobRewardedAd(REWARDED_NORMAL_UNIT_ID);
			_expensiveRewarded = new AdmobRewardedAd(REWARDED_EXPENSIVE_UNIT_ID);

            Initialize();
        }

        public void Initialize()
		{
			MobileAds.Initialize(initStatus =>
			{
				Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
				foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
				{
					string className = keyValuePair.Key;
					AdapterStatus status = keyValuePair.Value;
					switch (status.InitializationState)
					{
						case AdapterState.NotReady:
							// The adapter initialization did not complete.
							MonoBehaviour.print("Adapter: " + className + " not ready.");
							break;
						case AdapterState.Ready:
							// The adapter was successfully initialized.
							MonoBehaviour.print("Adapter: " + className + " is initialized.");
							break;
					}
				}
			});

			LoadInterstitial();
			LoadRewarded();
		}

		public async void ShowInterstitial()
		{
			// TO DO:
			// Check NoAds subscription
			if (_expensiveInterstitial.Show())
			{
				//_expensiveInterstitial.Load();
			}
			else if (_normalInterstitial.Show())
			{
				//_normalInterstitial.Load();
			}
			else if (_cheepInterstitial.Show())
			{
				//_cheepInterstitial.Load();
			}
			else
			{
				Debug.Log("InterstitialAd is not ready yet");
				for (int i = 0; i < 5; i++)
				{
					if (_expensiveInterstitial.Show())
					{
						//_expensiveInterstitial.Load();
						return;
					}
					else if (_normalInterstitial.Show())
					{
						//_normalInterstitial.Load();
						return;
					}
					else if (_cheepInterstitial.Show())
					{
						//_cheepInterstitial.Load();
						return;
					}
					LoadInterstitial();
                    await Task.Delay(500);
				}
			}
		}

		public async void ShowRewarded(Action<Reward> callback)
		{
			if (_expensiveRewarded.Show(callback))
				_expensiveRewarded.Load();
			else if (_normalRewarded.Show(callback))
				_normalRewarded.Load();
			else if (_cheepRewarded.Show(callback))
				_cheepRewarded.Load();
			else
			{
				Debug.Log("RewardedAd is not ready yet");
				for (int i = 0; i < 5; i++)
				{
					if (_expensiveRewarded.Show(callback))
					{
						_expensiveRewarded.Load();
						return;
					}
					else if (_normalRewarded.Show(callback))
					{
						_normalRewarded.Load();
						return;
					}
					else if (_cheepRewarded.Show(callback))
					{
						_cheepRewarded.Load();
						return;
					}
					await Task.Delay(500);
				}
			}
		}

		private void LoadInterstitial()
		{
			_expensiveInterstitial.Load();
			_normalInterstitial.Load();
			_cheepInterstitial.Load();
		}

		private void LoadRewarded()
		{
			_expensiveRewarded.Load();
			_normalRewarded.Load();
			_cheepRewarded.Load();
		}
	}
}