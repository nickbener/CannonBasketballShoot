using Services.Data.Crypto.ROS;
using UnityEditor;

namespace Services.Data.Editor
{
    public static class MenuExtension
    {
        [MenuItem("Tools/DataService/ClearAllDataModels")]
        public static void ClearAllDataModels()
        {
            new DataService().ClearAllDataModels();
        }
    }
}