using UnityEngine;

namespace RuntimeDebug.ConsoleMethods
{
    public static class Tools
    {
        public static bool IsDebug
        {
            get { return Application.version[0] == 'd'; }
        }
    }
}