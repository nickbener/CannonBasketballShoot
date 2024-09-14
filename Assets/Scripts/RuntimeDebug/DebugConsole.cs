using Management;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private GameObject _debugPanel;
    [SerializeField] private CompositionRoot _compositionRoot;
    public void DebugButton()
    {
        _debugPanel.SetActive(!_debugPanel.activeSelf);
    }
    public void RestartGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "Data/Models");
        DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        Application.Quit();
    }
    public void Add500Gold()
    {
        _compositionRoot.ResourceService.AppendResourceAmount(ResourceSystem.ResourceType.Gold, 500);
    }
}
