using UnityEngine.SceneManagement;
using UnityEngine;
using Management.Roots;
using Utilities;
using ResourceSystem;

namespace UI.Popups
{
    public class NotificationPopup : Popup
    {
        protected GameplaySceneRoot _gameplaySceneRoot;
        protected ResourceSystemService _resourceService;
        protected InterstitialAdvertisement _interstitialAdvertisement;

        public virtual void Initialize(GameplaySceneRoot gameplaySceneRoot, ResourceSystemService resourceSystemService, InterstitialAdvertisement interstitialAdvertisement)
        {
            _gameplaySceneRoot = gameplaySceneRoot;
            _resourceService = resourceSystemService;
            _interstitialAdvertisement = interstitialAdvertisement;
        }

        public virtual void RestartButton()
        {
            _gameplaySceneRoot.Dispose();
            this.Hide();
            SceneManager.LoadScene(ScenesMetadata.GameplaySceneName);
        }
        public virtual void OkButton()
        {
            _gameplaySceneRoot.Dispose();
            SceneManager.LoadScene(ScenesMetadata.LevelsSceneName);
            //_interstitialAdvertisement.ShowInterstitialAd();
        }
    }
}
