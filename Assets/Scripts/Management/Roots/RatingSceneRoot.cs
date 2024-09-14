using Infrastructure.Providers;
using Newtonsoft.Json.Bson;
using ResourceSystem;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RatingSceneRoot : MonoBehaviour
{
    [SerializeField] RandomRaitingGenerator _scoreRatingGenerator;
    [SerializeField] RandomRaitingGenerator _starRatingGenerator;
    [SerializeField] RandomRaitingGenerator _levelRatingGenerator;

    private ResourceSystemService _resourceSystemService;
    private PlayerDataProvider _playerDataProvider;

    [Inject]
    public void Inject(PlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;
    }

    public static RatingSceneRoot FromCurrentScene()
    {
        return FindObjectOfType<RatingSceneRoot>();
    }

    public RatingSceneRoot Initialize(
        ResourceSystemService resourceService)
    {
        if (string.IsNullOrEmpty(_playerDataProvider.SaveData.Nickname))
            ConstructNickName();
        _resourceSystemService = resourceService;
        Dictionary<RandomRaitingGenerator.RatingType, int> scoreAmount = GetAmountScore();
        _scoreRatingGenerator.Initialize(_playerDataProvider, resourceService, scoreAmount);
        _starRatingGenerator.Initialize(_playerDataProvider, resourceService, scoreAmount);
        _levelRatingGenerator.Initialize(_playerDataProvider, resourceService, scoreAmount);
        return this;
    }

    private void ConstructNickName()
    {
        _playerDataProvider.SaveData.Nickname = $"user_{string.Format("{0:D5}", Random.Range(100, 99999))}";
        _playerDataProvider.SaveDataToFile();
    }

    private Dictionary<RandomRaitingGenerator.RatingType, int> GetAmountScore()
    {
        Dictionary<RandomRaitingGenerator.RatingType, int> scoreMap = new Dictionary<RandomRaitingGenerator.RatingType, int>();
        scoreMap[RandomRaitingGenerator.RatingType.star] = (int)_resourceSystemService.GetResourceAmount(ResourceType.Star);
        scoreMap[RandomRaitingGenerator.RatingType.score] = (int)_resourceSystemService.GetResourceAmount(ResourceType.Score);
        scoreMap[RandomRaitingGenerator.RatingType.level] = _playerDataProvider.SaveData.LevelsRecord.Count;
        return scoreMap;
    }
}
