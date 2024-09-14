using Configs;
using Editor.LevelEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelDataProvider
{
    private LevelsConfigData _levelData;

    public LevelsConfigData LevelData => _levelData;

    public LevelDataProvider()
    {
        //var path = GetPath();

        //var file = File.ReadAllText(path);
        //_levelData = JsonUtility.FromJson<LevelsConfigData>(file);
        TextAsset text = Resources.Load<TextAsset>("levelsSaves");
        _levelData = JsonUtility.FromJson<LevelsConfigData>(text.text);
    }

    private string GetPath(string fleName = "levelsSaves")
    {
        return $"{Application.dataPath}/Resources/{fleName}.json";
    }
}

