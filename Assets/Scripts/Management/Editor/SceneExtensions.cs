using UnityEditor;
using UnityEditor.SceneManagement;

namespace Management.Editor
{
    public static class SceneExtensions
    {
        [MenuItem("Tools/Scenes/ToStartScene")]
        public static void OpenStartScene()
        {
            EditorApplication.ExecuteMenuItem("File/Save");
            EditorSceneManager.OpenScene("Assets/Scenes/Start.unity");
        }
    }
}