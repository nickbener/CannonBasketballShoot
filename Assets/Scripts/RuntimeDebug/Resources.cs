using IngameDebugConsole;
using Utils.Extensions;

namespace RuntimeDebug.ConsoleMethods
{
    public static class Resources
    {
        [ConsoleMethod("resource.set-g", "sets gold"), UnityEngine.Scripting.Preserve]
        public static void SetGold(long newGold)
        {
            //ZenjectExtensions.ProjectContextContainerContainer.Resolve<ResourceService>().Gold += newGold;
        }
    }
}