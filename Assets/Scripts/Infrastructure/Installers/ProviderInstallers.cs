using Infrastructure.Providers;
using Services.Data;
using Services.Data.Crypto.ROS;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class ProviderInstallers : MonoInstaller
    {
        [SerializeField] private RosCryptoConfig _cryptoConfig;

        public override void InstallBindings()
        {
            BindDataService();
            BindPlayerDataProvider();
            BindLevelDataProvider();
        }

        private void BindPlayerDataProvider()
        {
            Container.BindInterfacesAndSelfTo<PlayerDataProvider>().FromNew().AsSingle();
        }

        private void BindDataService()
        {
            if (_cryptoConfig.IsEnabled)
                Container.BindInterfacesAndSelfTo<DataService>().FromNew().AsSingle().WithArguments(_cryptoConfig.CryptoKey);
            else
                Container.BindInterfacesAndSelfTo<DataService>().FromNew().AsSingle();
        }

        private void BindLevelDataProvider()
        {
            Container.BindInterfacesAndSelfTo<LevelDataProvider>().FromNew().AsSingle();
        }
    }
}