using CodeBase;
using Configs;
using DG.Tweening;
using Editor.LevelEditor;
using Factories;
using Gameplay.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UI;
using UnityEngine;

public class BasketContainer : MonoBehaviour
{
    private BasketFactory _basketFactory;
    private LevelEditorSettings _levelEditorSettings;
    private LevelData _levelConfigData;

    private EventBusService _eventBusService;
    private Basket _currentBasket;
    private int _currentId;

    public void Initialize(LevelData levelConfigData, LevelEditorSettings levelEditorSettings, BasketFactory basketFactory, EventBusService eventBusService)
    {
        _basketFactory = basketFactory;
        _levelEditorSettings = levelEditorSettings;
        _eventBusService = eventBusService;
        _levelConfigData = levelConfigData;

        CreateBasket(_levelConfigData.Throws[_currentId++]);
        _eventBusService.AddListener(GameEventKey.GoalScored, OnGoalScored);
    }

    private void OnDestroy()
    {
        _eventBusService.RemoveListener(GameEventKey.GoalScored, OnGoalScored);
    }

    private void CreateBasket(ThrowConfigData throwData)
    {
        List<Vector3> pathPoints = new List<Vector3>();
        BasketConfigData basketConfigData = throwData.Baskets[0];
        foreach (Vector3 item in basketConfigData.Positions)
        {
            pathPoints.Add(item);
        }

        if (basketConfigData.BasketType == Basket.BasketType.front)
        {
            _currentBasket = _basketFactory.CreateFrontBasket(basketConfigData.Positions[0],
                basket => basket.transform.DOPath(pathPoints.ToArray(), basketConfigData.cycleTime).SetEase(Ease.Linear).SetLoops(-1).SetLink(basket.gameObject));
        }
        else
        {
            List<Vector3> newPathPoints = new List<Vector3>();
            foreach (var item in pathPoints)
            {
                float x = 0;
                if (basketConfigData.BasketSide == BasketSide.Left)
                    x = _levelEditorSettings.BasketLeftPosition.x;
                else
                    x = _levelEditorSettings.BasketRightPosition.x;

                newPathPoints.Add(new Vector3(x, item.y, item.z));
            }

            _currentBasket = _basketFactory.CreateBasket(basketConfigData.BasketSide, basketConfigData.Positions[0].y,
                basket => basket.transform.DOPath(newPathPoints.ToArray(), basketConfigData.cycleTime).SetEase(Ease.Linear).SetLoops(-1).SetLink(basket.gameObject));
        }
        _currentBasket.DoShow();
    }
    private void OnGoalScored(object sender, EventArgs args)
    {
        if (_levelConfigData.Throws.Count > _currentId)
        {
            _currentBasket.DoHide();
            CreateBasket(_levelConfigData.Throws[_currentId++]);
        }  
    }
}
