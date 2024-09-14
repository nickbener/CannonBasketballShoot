using System;
using System.Globalization;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;

namespace Infrastructure.Services.Analytics
{
    public class FirebaseAnalyticsLogService : IAnalyticsLogService
    {
        public bool IsInitialized { get; set; }

        public FirebaseAnalyticsLogService()
        {
            IsInitialized = false;
            Initialize();
        }
        public async UniTask Initialize()
        {
            await ResolveDependenciesAndInitialize();
            IsInitialized = true;
        }

        public void LogEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
        }

        private async UniTask ResolveDependenciesAndInitialize()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {

            }
        }

        private void InitializeFirebase()
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
            

#if UNITY_EDITOR || DEVELOPMENT_BUILD || DEV
            FirebaseAnalytics.SetUserProperty("DEV", true.ToString(CultureInfo.InvariantCulture));
#else
            FirebaseAnalytics.SetUserProperty("DEV", false.ToString(CultureInfo.InvariantCulture));
#endif
            
#if UNITY_EDITOR
            FirebaseAnalytics.SetUserId($"DEVELOPER_{Environment.UserName}");
#elif DEV
            FirebaseAnalytics.SetUserId($"TESTER_{UnityEngine.Device.SystemInfo.deviceUniqueIdentifier}");
#else
            FirebaseAnalytics.SetUserId($"USER_{UnityEngine.Device.SystemInfo.deviceUniqueIdentifier}");
#endif
            FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));

            LogEvent(FirebaseAnalytics.EventLogin);
#if DEV
            LogEvent("SetDEV");
#endif
        }
    }
}