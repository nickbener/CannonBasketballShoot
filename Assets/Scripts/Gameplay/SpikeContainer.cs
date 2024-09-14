using CodeBase;
using Editor.LevelEditor;
using System;
using UnityEngine;

public class SpikeContainer : MonoBehaviour
{
    [SerializeField] private BonusManager _bonusManager;
    private LevelData _levelConfigData;
    private EventBusService _eventBusService;
    private LevelEditorSettings _levelEditorSettings;
    private int _currentId;
    public void Initialize(LevelData levelConfigData, LevelEditorSettings levelEditorSettings, EventBusService eventBusService)
    {
        _levelConfigData = levelConfigData;
        _eventBusService = eventBusService;
        _levelEditorSettings = levelEditorSettings;
        InitializeSpikeByThrow(levelConfigData.Throws[_currentId++]);
        _eventBusService.AddListener(GameEventKey.GoalScored, OnGoalScored);
    }

    private void OnDestroy()
    {
        _eventBusService.RemoveListener(GameEventKey.GoalScored, OnGoalScored);
    }
    private void InitializeSpikeByThrow(ThrowConfigData throwData)
    {
        CleanChildren(transform);
        _bonusManager.BoxInSpike.Clear();
        foreach (SpikeConfigData item in throwData.Spikes)
        {
            GameObject spike = Instantiate(_levelEditorSettings.SpikePrefab, transform);
            if (item.Side == Gameplay.Environment.BorderType.Left)
                spike.transform.rotation = Quaternion.Euler(_levelEditorSettings.SpikeLeftRotation);
            else if (item.Side == Gameplay.Environment.BorderType.Right)
                spike.transform.rotation = Quaternion.Euler(_levelEditorSettings.SpikeRightRotation);
            else if (item.Side == Gameplay.Environment.BorderType.Top)
                spike.transform.rotation = Quaternion.Euler(_levelEditorSettings.SpikeTopRotation);

            spike.transform.position = item.Position;
            _bonusManager.BoxInSpike.Add(spike.GetComponent<Spike>().BlockBox);
        }
        _bonusManager.ShieldButton(LevelSettings.BustSelected[ResourceSystem.ResourceType.BustDef]);
    }
    private void OnGoalScored(object sender, EventArgs args)
    {
        if (_levelConfigData.Throws.Count > _currentId)
            InitializeSpikeByThrow(_levelConfigData.Throws[_currentId++]);
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
