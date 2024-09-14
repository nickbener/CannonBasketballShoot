using Game.Monetization.Ads;
using Infrastructure.Services.Analytics;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindAtalyticsService();
            BindAdsService();
        }

        private void BindAtalyticsService()
        {
            Debug.Log("FirebaseAnalyticsLogService");
            Container.BindInterfacesTo<FirebaseAnalyticsLogService>().FromNew().AsSingle().NonLazy();
        }
        private void BindAdsService()
        {
            Container.BindInterfacesTo<AdmobAdsService>().FromNew().AsSingle();
        }
    }
}