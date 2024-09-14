//using System;
//using System.Collections;
//using System.Collections.Generic;
//using GoogleMobileAds.Api;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//namespace Managers
//{
//    public class AdsManager : MonoBehaviour
//    {
//        public static AdsManager Instance { get; private set; }

//        private const string ADUnitRewardExpensive = "ca-app-pub-8340576279106634/7159881359";
//        private const string ADUnitRewardNormal = "ca-app-pub-8340576279106634/9043808665";
//        private const string ADUnitRewardCheep = "ca-app-pub-8340576279106634/9769162003";
        
//        private const string ADUnitInterstitialExpensive = "ca-app-pub-8340576279106634/4053800891";
//        private const string ADUnitInterstitialNormal = "ca-app-pub-8340576279106634/7646966519";
//        private const string ADUnitInterstitialCheep = "ca-app-pub-8340576279106634/3324578125";

//        private const string ADUnitBannerExpensive = "ca-app-pub-8340576279106634/9211329629";
        
//        public static bool isGrowInterstitial = true;
//        public static bool isEvolveInterstitial = true;

//        private Coroutine _load;

//        [SerializeField]
//        private GameObject _notFoundReward;

//        private void Awake()
//        {
//            if (Instance != null)
//            {
//                Destroy(gameObject);
//                return;
//            }
//            else
//            {
//                Instance = this;
//                DontDestroyOnLoad(gameObject);
//            }

//            //MobileAds.Initialize((initStatus) =>
//            //{
//            //    Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
//            //    foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
//            //    {
//            //        string className = keyValuePair.Key;
//            //        AdapterStatus status = keyValuePair.Value;
//            //        switch (status.InitializationState)
//            //        {
//            //            case AdapterState.NotReady:
//            //                // The adapter initialization did not complete.
//            //                MonoBehaviour.print("Adapter: " + className + " not ready.");
//            //                break;
//            //            case AdapterState.Ready:
//            //                // The adapter was successfully initialized.
//            //                MonoBehaviour.print("Adapter: " + className + " is initialized.");
//            //                break;
//            //        }
//            //    }
//            //});
            
//            InitInterstitial();
//            //InitRewarded();
            
//            RequestBanner();
//            SceneManager.sceneLoaded += (scene, mode) =>
//            {
//                RequestBanner();
//            };
            
//            Debug.Log("Ads init successful");
//        }

//        #region Interstital
//        private InterstitialAd _interstitialExpensive;
//        private InterstitialAd _interstitialNormal;
//        private InterstitialAd _interstitialCheep;

//        private void InitInterstitial()
//        {
//            RequestInterstitial();
//        }
//        AdRequest requestExp;
//        AdRequest requestNormal;
//        AdRequest requestCheep;

//        private void RequestInterstitialExpensive()
//        {
//            requestExp = new AdRequest();
//            InterstitialAd.Load(ADUnitInterstitialExpensive, requestExp, (interstitial, error) =>
//            {
//                _interstitialExpensive = interstitial;
//            });
//        }
        
//        private void RequestInterstitialNormal()
//        {
//            requestNormal = new AdRequest();
//            InterstitialAd.Load(ADUnitInterstitialNormal, requestNormal,  (interstitial, error) =>
//            {
//                _interstitialNormal = interstitial;
//            });
//        }
        
//        private void RequestInterstitialCheep()
//        {
//            requestCheep = new AdRequest();
//            InterstitialAd.Load(ADUnitInterstitialCheep, requestCheep,  (interstitial, error) =>
//            {
//                _interstitialCheep = interstitial;
//            });
//        }

//        private void RequestInterstitial()
//        {
//            RequestInterstitialExpensive();
//            RequestInterstitialNormal();
//            RequestInterstitialCheep();
//        }


//        public void ShowInterstitial()
//        {
//            //if (IAPManager.IsSubscribeEnable) return;
//            if (_interstitialExpensive.CanShowAd())
//            {
//                _interstitialExpensive.Show();
//                RequestInterstitialExpensive();
//            }
//            else if (this._interstitialNormal.CanShowAd())
//            {
//                _interstitialNormal.Show();
//                RequestInterstitialNormal();
//            }
//            else if (this._interstitialCheep.CanShowAd())
//            {
//                _interstitialCheep.Show();
//                RequestInterstitialCheep();
//            }
//            else
//            {
//                Debug.Log("Interstitial is not ready yet");
//                StartCoroutine(TryToLoadInterstitialVideo());
//            }
//        }


//        private IEnumerator TryToLoadInterstitialVideo()
//        {
//            for (int i = 0; i < 5; i++)
//            {
//                if (this._interstitialExpensive.CanShowAd())
//                {
//                    _interstitialExpensive.Show();
//                    RequestInterstitialExpensive();
//                    yield break;
//                }
//                else if (this._interstitialNormal.CanShowAd())
//                {
//                    _interstitialNormal.Show();
//                    RequestInterstitialNormal();
//                    yield break;
//                }
//                else if (this._interstitialCheep.CanShowAd())
//                {
//                    _interstitialCheep.Show();
//                    RequestInterstitialCheep();
//                    yield break;
//                }

//                yield return new WaitForSeconds(0.5f);
//            }
//        }
//        #endregion

//        #region Rewarded
//        private Reward _rewardedAdExpensive;
//        private Reward _rewardedAdNormal;
//        private Reward _rewardedAdCheep;

//        private void InitRewarded()
//        {
//            RequestRewarded();
//        }
        
//        AdRequest requestExpensiveReward;
//        AdRequest requestNormalReward;
//        AdRequest requestCheepReward;

//        private void RequestRewardedExpensive()
//        {
//            requestExpensiveReward = new AdRequest();
//            Reward.Load(ADUnitRewardExpensive, requestExpensiveReward,
//                (ad, error) =>
//                {
//                    _rewardedAdExpensive = ad;
//                    _rewardedAdExpensive.OnAdFullScreenContentClosed += RequestRewarded;
//                });
//        }
        
//        private void RequestRewardedNormal()
//        {
//            requestNormalReward = new AdRequest();
//            RewardedAd.Load(ADUnitRewardNormal, requestNormalReward,
//                (ad, error) =>
//                {
//                    _rewardedAdNormal = ad;
//                    _rewardedAdNormal.OnAdFullScreenContentClosed += RequestRewarded;
//                });
//        }

//        private void RequestRewardedCheep()
//        {
//            requestCheepReward = new AdRequest();
//            RewardedAd.Load(ADUnitRewardCheep, requestCheepReward,
//                (ad, error) =>
//                {
//                    _rewardedAdCheep = ad;
//                    _rewardedAdCheep.OnAdFullScreenContentClosed += RequestRewarded;
//                });
//        }

//        private void RequestRewarded()
//        {
//            RequestRewardedExpensive();
//            RequestRewardedNormal();
//            RequestRewardedCheep();
//        }


//        public void ShowRewardedAd(Action<Reward> action = null)
//        {
//            if (_rewardedAdExpensive.CanShowAd())
//            {
//                _rewardedAdExpensive.Show(action);
//                RequestRewardedExpensive();
//            }
//            else if (_rewardedAdNormal.CanShowAd())
//            {
//                _rewardedAdNormal.Show(action);
//                RequestInterstitialNormal();
//            }
//            else if (_rewardedAdCheep.CanShowAd())
//            {
//                _rewardedAdCheep.Show(action);
//                RequestInterstitialCheep();
//            }
//            else
//            {
//                _notFoundReward.SetActive(true);
//                Invoke(nameof(RewardNotLoadDisable), 2f);
//                Debug.Log("RewardNotLoad");
//            }
//        }

//        private void RewardNotLoadDisable()
//        {
//            _notFoundReward.SetActive(false);
//        }
//        #endregion

//        //private IEnumerator TryToLoadRewardVideo(Action action = null)
//        //{
//        //    if (_rewardLoadingScreen == null) { _rewardLoadingScreen = Instantiate(_rewardLoadingScreenPrefab, GameObject.FindGameObjectWithTag("Boards").transform); }
//        //    else
//        //    {
//        //        _rewardLoadingScreen.SetActive(true);
//        //    }
//        //    for (int i = 0; i < 5; i++)
//        //    {
//        //        if (_rewardedAd.IsLoaded())
//        //        {
//        //            _rewardedAd.Show();
//        //            _actionEarn = action;
//        //            _rewardLoadingScreen.SetActive(false);
//        //            _load = null;
//        //            yield break;
//        //        }
//        //        yield return new WaitForSeconds(0.5f);
//        //    }
//        //    _rewardLoadingScreen.SetActive(false);
//        //    _load = null;
//        //    if (_error == null) { _error = Instantiate(_errorPrefab, GameObject.FindGameObjectWithTag("Boards").transform); }
//        //    else
//        //    {
//        //        _error.SetActive(true);
//        //    }
//        //    yield return new WaitForSeconds(2f);
//        //    _error.SetActive(false);
//        //}

//        #region Banner

//        private BannerView _bannerView;
        
//        private void RequestBanner()
//        {
//            // Create a 320x50 banner at the top of the screen.
//            _bannerView = new BannerView(ADUnitBannerExpensive, AdSize.Banner, AdPosition.Top);
            
//            // Create an empty ad request.
//            AdRequest request = new AdRequest();
            
//            // Load the banner with the request.
//            _bannerView.LoadAd(request);
//        }

//        #endregion
//    }
//}
