using CodeBase;
using Editor.LevelEditor;
using Gameplay.Environment;
using ResourceSystem;
using System;
using UnityEngine;

public class StarContainer : MonoBehaviour
{
    private LevelData _levelConfigData;
    private EventBusService _eventBusService;
    private LevelEditorSettings _levelEditorSettings;
    private ResourceSystemService _resourceService;
    private int _currentId;
    public void Initialize(LevelData levelConfigData, LevelEditorSettings levelEditorSettings, ResourceSystemService resourceService, EventBusService eventBusService)
    {
        _levelConfigData = levelConfigData;
        _eventBusService = eventBusService;
        _levelEditorSettings = levelEditorSettings;
        _resourceService = resourceService;
        InitializeStarByThrow(levelConfigData.Throws[_currentId++]);
        _eventBusService.AddListener(GameEventKey.GoalScored, OnGoalScored);
    }

    private void OnDestroy()
    {
        _eventBusService.RemoveListener(GameEventKey.GoalScored, OnGoalScored);
    }
    private void InitializeStarByThrow(ThrowConfigData throwData)
    {
        CleanChildren(transform);
        foreach (StarConfigData item in throwData.Stars)
        {
            GameObject star = Instantiate(_levelEditorSettings.StarPrefab, item.Position, Quaternion.identity, transform);
            star.GetComponent<Star>().Initialize(_resourceService);
        }
    }
    private void OnGoalScored(object sender, EventArgs args)
    {
        if (_levelConfigData.Throws.Count > _currentId)
            InitializeStarByThrow(_levelConfigData.Throws[_currentId++]);
    }

    private void CleanChildren(Transform parent)
    {
        int nbChildren = parent.childCount;
        for (int i = nbChildren - 1; i >= 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
