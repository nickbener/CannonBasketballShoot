using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Analytics
{
    public interface IAnalyticsLogService
    {
        public bool IsInitialized { get; set; }
        UniTask Initialize();

        void LogEvent(string eventName);
    }
}