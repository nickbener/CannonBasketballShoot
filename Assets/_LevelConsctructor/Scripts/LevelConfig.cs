using UnityEngine;
using TriInspector;
using System.IO;
using System.Linq;
using Gameplay.Environment;
using Gameplay;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine.UIElements;
using DG.Tweening;
using AYellowpaper.SerializedCollections;
using Editor.LevelEditor;

[CreateAssetMenu(fileName = "LevelConfigurator", menuName = "Custom/LevelConfigurator")]
[DeclareBoxGroup("cannon", Title = "Cannon")]
[DeclareHorizontalGroup("cannon/horizontal")]
[DeclareBoxGroup("cannon/horizontal/left", Title = "Left")]
[DeclareBoxGroup("cannon/horizontal/right", Title = "Right")]

[DeclareBoxGroup("basket", Title = "Basket")]
[DeclareHorizontalGroup("basket/horizontal")]
[DeclareBoxGroup("basket/horizontal/side", Title = "Side Basket")]
[DeclareBoxGroup("basket/horizontal/front", Title = "Front Basket")]

[DeclareBoxGroup("spike", Title = "Spike")]
[DeclareBoxGroup("star", Title = "Star")]
[DeclareBoxGroup("generate", Title = "Generate")]
[DeclareBoxGroup("combinethrows", Title = "Combine Throws to levels")]

public class LevelConfig : ScriptableObject
{
    [Title("Settings")]
    [SerializeField][PropertyOrder(10000)] private LevelEditorSettings _settings;

    [Title("Create level")]
    [PropertyOrder(1)]
    public ThrowConfigData ThrowData;
    [PropertyOrder(1)]
    [Group("cannon")]
    public int IdCannonPivot;
    [PropertyOrder(1)]
    [Group("cannon")]
    public int IdCannonMain;


    [PropertyOrder(1)]
    [Group("basket")]
    [Dropdown(nameof(_existingBaskets))]
    public BasketColor EditingBasket;
    [PropertyOrder(1)]
    [Group("basket")]
    public SerializedDictionary<BasketColor, BasketConfigData> BasketsData = new();

    //[Group("generate")]
    //[PropertyOrder(2)]
    //[SerializeField] private int _chanceLeftCannon;

    [Space]
    [Title("Load level")]
    //[PropertyOrder(50)]
    //[Dropdown(nameof(_existingLevels))]
    //public int Level;
    private int Level;
    [PropertyOrder(51)]
    [Header("Load level")]
    public TextAsset _textFile;

    [Space]
    //[Title("Combine Throws to levels")]
    [Group("combinethrows")]
    [PropertyOrder(2050)]
    public int LevelNumber;
    [Group("combinethrows")]
    [PropertyOrder(2050)]
    public int IdBackgroundSprite;
    [Group("combinethrows")]
    [PropertyOrder(2050)]
    public List<TextAsset> Throws;


    [Title("Create Levels File")]
    [PropertyOrder(3000)]
    public SerializedDictionary<int, TextAsset> levelsFiles = new();

    private GameObject _basketContainer;
    private GameObject _spikeContainer;
    private GameObject _starContainer;
    [SerializeField]
    [HideInInspector]
    private Dictionary<BasketColor, GameObject> _basketMap = new();
    private Dictionary<BasketColor, Color> _colorMap = new()
        {
            {BasketColor.Blue, Color.blue },
            {BasketColor.Green, Color.green },
            {BasketColor.Red, Color.red }
        };
    private BasketColor[] _existingBaskets
    {
        get
        {
            List<BasketColor> basketColors = new List<BasketColor>();
            foreach (Transform item in _basketContainer.transform)
            {
                if (item.gameObject.GetComponent<GizmosDraw>().GizmosColor == Color.blue)
                {
                    basketColors.Add(BasketColor.Blue);
                    _basketMap[BasketColor.Blue] = item.gameObject;

                    if (!BasketsData.ContainsKey(BasketColor.Blue))
                        BasketsData[BasketColor.Blue] = new BasketConfigData();
                }
                else if (item.gameObject.GetComponent<GizmosDraw>().GizmosColor == Color.red)
                {
                    basketColors.Add(BasketColor.Red);
                    _basketMap[BasketColor.Red] = item.gameObject;

                    if (!BasketsData.ContainsKey(BasketColor.Red))
                        BasketsData[BasketColor.Red] = new BasketConfigData();
                }
                else if (item.gameObject.GetComponent<GizmosDraw>().GizmosColor == Color.green)
                {
                    basketColors.Add(BasketColor.Green);
                    _basketMap[BasketColor.Green] = item.gameObject;

                    if (!BasketsData.ContainsKey(BasketColor.Green))
                        BasketsData[BasketColor.Green] = new BasketConfigData();
                }
            }
            return basketColors.ToArray();
        }
        set { }
    }
    private int[] _existingLevels;

    private async void OnEnable()
    {
        Debug.Log("OnEnable");
    }

    private async void OnValidate()
    {
        Debug.Log(GetPath());
        if (_basketContainer == null)
            _basketContainer = GameObject.Find("Baskets");
        if (_spikeContainer == null)
            _spikeContainer = GameObject.Find("Spikes");
        if (_starContainer == null)
            _starContainer = GameObject.Find("Stars");

        Debug.Log("Node On Validate");
    }

    [Button(Name = "Create Left Cannon")]
    [Group("cannon/horizontal/left")]
    [PropertyOrder(2)]
    public void CreateLeftCannonButton()
    {
        GameObject bottomPlatform = GameObject.Find("bottomPlatform");
        GameObject cannon = GameObject.Find("Cannon");

        bottomPlatform.transform.rotation = Quaternion.Euler(new Vector3(bottomPlatform.transform.eulerAngles.x, 0, bottomPlatform.transform.eulerAngles.z));
        cannon.transform.position = _settings.CannonLeftPosition;
        cannon.GetComponent<Cannon>().Pivot.transform.rotation = Quaternion.Euler(new Vector3(cannon.transform.eulerAngles.x, cannon.transform.eulerAngles.y, -30));
        //cannon.transform.rotation = Quaternion.Euler(new Vector3(cannon.transform.eulerAngles.x, 0, cannon.transform.eulerAngles.z));
    }

    [Button(Name = "Create Right Cannon")]
    [Group("cannon/horizontal/right")]
    [PropertyOrder(2)]
    public void CreateRightCannonButton()
    {
        GameObject bottomPlatform = GameObject.Find("bottomPlatform");
        GameObject cannon = GameObject.Find("Cannon");

        bottomPlatform.transform.rotation = Quaternion.Euler(new Vector3(bottomPlatform.transform.eulerAngles.x, 180f, bottomPlatform.transform.eulerAngles.z));
        cannon.transform.position = _settings.CannonRightPosition;
        cannon.GetComponent<Cannon>().Pivot.transform.rotation = Quaternion.Euler(new Vector3(cannon.transform.eulerAngles.x, cannon.transform.eulerAngles.y, 30));
        //cannon.transform.rotation = Quaternion.Euler(new Vector3(cannon.transform.eulerAngles.x, 180f, cannon.transform.eulerAngles.z));
    }
    // BASKET
    [Button(Name = "Create Left Basket")]
    [Group("basket/horizontal/side")]
    [PropertyOrder(2)]
    public void CreateLeftSideBasketButton()
    {
        if (TryGetEmptySlotBasket(out BasketColor emptySlot))
        {
            GameObject basket = Instantiate(_settings.BasketSidePrefab, _basketContainer.transform);
            basket.GetComponent<Basket>().Side = BasketSide.Left;
            basket.transform.position = _settings.BasketLeftPosition;
            basket.transform.rotation = Quaternion.Euler(new Vector3(basket.transform.eulerAngles.x, 0, basket.transform.eulerAngles.z));

            basket.GetComponent<GizmosDraw>().GizmosColor = _colorMap[emptySlot];
            _basketMap[emptySlot] = basket;

            BasketsData[emptySlot] = new BasketConfigData();
        }
    }

    [Button(Name = "Create Right Basket")]
    [Group("basket/horizontal/side")]
    [PropertyOrder(2)]
    public void CreateRightSideBasketButton()
    {
        if (TryGetEmptySlotBasket(out BasketColor emptySlot))
        {
            GameObject basket = Instantiate(_settings.BasketSidePrefab, _basketContainer.transform);
            basket.GetComponent<Basket>().Side = BasketSide.Right;
            basket.transform.position = _settings.BasketRightPosition;
            basket.transform.rotation = Quaternion.Euler(new Vector3(basket.transform.eulerAngles.x, 180, basket.transform.eulerAngles.z));

            basket.GetComponent<GizmosDraw>().GizmosColor = _colorMap[emptySlot];
            _basketMap[emptySlot] = basket;

            BasketsData[emptySlot] = new BasketConfigData();
        }
    }

    [Button(Name = "Create Front Basket")]
    [Group("basket/horizontal/front")]
    [PropertyOrder(2)]
    public void CreateFrontBasketButton()
    {
        if (TryGetEmptySlotBasket(out BasketColor emptySlot))
        {
            GameObject basket = Instantiate(_settings.BasketFrontPrefab, _basketContainer.transform);
            basket.transform.position = _settings.BasketFrontPosition;

            basket.GetComponent<GizmosDraw>().GizmosColor = _colorMap[emptySlot];
            _basketMap[emptySlot] = basket;

            BasketsData[emptySlot] = new BasketConfigData();
        }
    }

    [Button(Name = "Delete Selected Basket")]
    [Group("basket")]
    [PropertyOrder(2)]
    public void DeleteSelectedBasketButton()
    {
        if (_basketMap.ContainsKey(EditingBasket))
        {
            DestroyImmediate(_basketMap[EditingBasket]);
            _basketMap.Remove(EditingBasket);
            BasketsData.Remove(EditingBasket);

        }
    }

    [Button(Name = "Create Basket Move Point")]
    [Group("basket")]
    [PropertyOrder(2)]
    public GameObject CreateBasketPointBasketButton()
    {
        if (!_basketMap.ContainsKey(EditingBasket))
            return null;

        Basket editingBasket = _basketMap[EditingBasket].GetComponent<Basket>();
        GameObject pathPoint = Instantiate(_settings.PointPrefab, editingBasket.TrajectoryContainer.transform);
        pathPoint.GetComponent<GizmosDraw>().GizmosColor = _colorMap[EditingBasket];
        return pathPoint;
    }

    [Button(Name = "Play Basket Animation")]
    [Group("basket")]
    [PropertyOrder(2)]
    public void PlayBasketAnimationButton()
    {
        if (!_basketMap.ContainsKey(EditingBasket))
            return;

        Basket editingBasket = _basketMap[EditingBasket].GetComponent<Basket>();

        if (editingBasket.TrajectoryContainer.transform.childCount == 0)
            return;

        List<Vector3> pathPoints = new List<Vector3>();
        pathPoints.Add(editingBasket.transform.position);
        foreach (Transform item in editingBasket.TrajectoryContainer.transform)
        {
            pathPoints.Add(item.transform.position);
        }
        pathPoints.Add(editingBasket.transform.position);

        if (editingBasket.Type == Basket.BasketType.side)
        {
            List<Vector3> newPathPoints = new List<Vector3>();
            foreach (var item in pathPoints)
            {
                newPathPoints.Add(new Vector3(_basketMap[EditingBasket].transform.position.x, item.y, _basketMap[EditingBasket].transform.position.z));
            }
            _basketMap[EditingBasket].transform.DOPath(newPathPoints.ToArray(), BasketsData[EditingBasket].cycleTime, PathType.Linear, PathMode.Sidescroller2D).SetEase(Ease.Linear);
        }
        else
            _basketMap[EditingBasket].transform.DOPath(pathPoints.ToArray(), BasketsData[EditingBasket].cycleTime, PathType.Linear, PathMode.Sidescroller2D).SetEase(Ease.Linear);
    }

    [Button(Name = "Create Left Spike")]
    [Group("spike")]
    [PropertyOrder(1)]
    public GameObject CreateLeftSpikeButton()
    {
        GameObject spike = Instantiate(_settings.SpikePrefab, _spikeContainer.transform);
        spike.name = "Spike Left";
        spike.GetComponent<Spike>().Type = BorderType.Left;
        spike.transform.position = _settings.SpikeLeftPosition;
        spike.transform.rotation = Quaternion.Euler(_settings.SpikeLeftRotation);
        return spike;
    }

    [Button(Name = "Create Right Spike")]
    [Group("spike")]
    [PropertyOrder(1)]
    public GameObject CreateRightSpikeButton()
    {
        GameObject spike = Instantiate(_settings.SpikePrefab, _spikeContainer.transform);
        spike.name = "Spike Right";
        spike.GetComponent<Spike>().Type = BorderType.Right;
        spike.transform.position = _settings.SpikeRightPosition;
        spike.transform.rotation = Quaternion.Euler(_settings.SpikeRightRotation);
        return spike;
    }

    [Button(Name = "Create Top Spike")]
    [Group("spike")]
    [PropertyOrder(1)]
    public GameObject CreateTopSpikeButton()
    {
        GameObject spike = Instantiate(_settings.SpikePrefab, _spikeContainer.transform);
        spike.name = "Spike Top";
        spike.GetComponent<Spike>().Type = BorderType.Top;
        spike.transform.position = _settings.SpikeTopPosition;
        spike.transform.rotation = Quaternion.Euler(_settings.SpikeTopRotation);
        return spike;
    }

    [Button(Name = "Create Star")]
    [Group("star")]
    [PropertyOrder(1)]
    public GameObject CreateStarButton()
    {
        GameObject star = Instantiate(_settings.StarPrefab, _starContainer.transform);
        return star;
    }

    [Button(Name = "Update Scene")]
    [PropertyOrder(2)]
    public void UpdateSceneButton()
    {
        //LEVEL
        GameObject levelBg = GameObject.Find("background");
        levelBg.GetComponent<SpriteRenderer>().sprite = _settings.SpriteAsset.GetSprite("Background", $"{IdBackgroundSprite}");
        //CANNON
        GameObject cannon = GameObject.Find("Cannon");
        ThrowData.Cannon.IdCannonPivot = IdCannonPivot;
        ThrowData.Cannon.IdCannonMain = IdCannonMain;
        if (cannon.transform.position == _settings.CannonLeftPosition)
            ThrowData.Cannon.Type = BasketSide.Left;
        else
            ThrowData.Cannon.Type = BasketSide.Right;

        cannon.GetComponent<Cannon>().Initialize(ThrowData, _settings);
        //LoadLevelButton();
    }

    [Button(ButtonSizes.Large, Name = "Save Throw")]
    [GUIColor("#00FF00")]
    [PropertyOrder(2)]
    public void SaveThrowButton()
    {
        //CANNON
        GameObject cannon = GameObject.Find("Cannon");
        ThrowData.Cannon.IdCannonPivot = IdCannonPivot;
        ThrowData.Cannon.IdCannonMain = IdCannonMain;
        if (cannon.transform.position == _settings.CannonLeftPosition)
            ThrowData.Cannon.Type = BasketSide.Left;
        else
            ThrowData.Cannon.Type = BasketSide.Right;
        //BACKET
        ThrowData.Baskets.Clear();
        if (_basketContainer == null)
            _basketContainer = GameObject.Find("Baskets");
        foreach (var item in _basketMap)
        {
            Basket currentBasket = _basketMap[item.Key].GetComponent<Basket>();
            BasketsData[item.Key].BasketType = currentBasket.Type;
            BasketsData[item.Key].BasketSide = currentBasket.Side;
            if (currentBasket.TrajectoryContainer.transform.childCount >= 0)
            {
                List<Vector3> pathPoints = new List<Vector3>();
                pathPoints.Add(currentBasket.transform.position);
                foreach (Transform point in currentBasket.TrajectoryContainer.transform)
                {
                    pathPoints.Add(point.transform.position);
                }
                pathPoints.Add(currentBasket.transform.position);

                BasketsData[item.Key].Positions = pathPoints;
            }
            ThrowData.Baskets.Add(BasketsData[item.Key]);
        }
        //SPIKE
        if (_spikeContainer == null)
            _spikeContainer = GameObject.Find("Spikes");
        ThrowData.Spikes.Clear();
        foreach (Transform item in _spikeContainer.transform)
        {
            ThrowData.Spikes.Add(new SpikeConfigData(item.gameObject.GetComponent<Spike>().Type, item.position));
        }
        //STAR
        if (_starContainer == null)
            _starContainer = GameObject.Find("Stars");
        ThrowData.Stars.Clear();
        foreach (Transform item in _starContainer.transform)
        {
            ThrowData.Stars.Add(new StarConfigData(item.transform.position));
        }
        //SAVE TO FILE
        var pathSepFile = GetPath(ThrowData.throwNumber);
        var serializedObjectSepFile = JsonUtility.ToJson(ThrowData);
        File.WriteAllText(pathSepFile, serializedObjectSepFile);
        //AssetDatabase.Refresh(); // Comment for build 423, 591
    }
    [Button(ButtonSizes.Large, Name = "CLEAR SCENE")]
    [GUIColor("#ff0000")]
    [PropertyOrder(2)]
    public async void ClearSceneButton()
    {
        //BASKETS
        if (_basketContainer == null)
            _basketContainer = GameObject.Find("Baskets");
        CleanChildren(_basketContainer.transform);
        _basketMap.Clear();
        BasketsData.Clear();
        //SPIKES
        if (_spikeContainer == null)
            _spikeContainer = GameObject.Find("Spikes");
        CleanChildren(_spikeContainer.transform);
        //STARS
        if (_starContainer == null)
            _starContainer = GameObject.Find("Stars");
        CleanChildren(_starContainer.transform);
    }
    #region Generate
    [Button(ButtonSizes.Large, Name = "GENERATE THROW")]
    [GUIColor("#ffff00")]
    [PropertyOrder(2)]
    public void GenerateThrowButton()
    {
        SaveThrowButton();
        ClearSceneButton();
        ThrowData.throwNumber = "generated";
        //CANNON
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            CreateLeftCannonButton();
        }
        else
        {
            CreateRightCannonButton();
        }
        //BASKETS
        Vector3 newPos = GetNewPosition();
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                CreateLeftSideBasketButton();
                newPos = new Vector3(_basketMap[EditingBasket].transform.position.x, newPos.y);
                break;
            case 1:
                CreateRightSideBasketButton();
                newPos = new Vector3(_basketMap[EditingBasket].transform.position.x, newPos.y);
                break;
            case 2:
                CreateFrontBasketButton();
                break;
        }
        _basketMap[EditingBasket].transform.position = newPos;
        int amountPathPoints = UnityEngine.Random.Range(0, 6);
        for (int i = 0; i < amountPathPoints; i++)
        {
            GameObject point = CreateBasketPointBasketButton();
            point.transform.position = GetNewPosition();
        }
        int cycleTime = UnityEngine.Random.Range(2, 11);
        BasketsData[EditingBasket].cycleTime = cycleTime;
        //SPIKES
        //left
        Dictionary<Vector2, GameObject> spikesLeft = new Dictionary<Vector2, GameObject>()
        {
            [new Vector2(-1.8f, 0.2f)] = null,
            [new Vector2(-1.8f, 1.05f)] = null,
            [new Vector2(-1.8f, 1.9f)] = null,
            [new Vector2(-1.8f, 2.75f)] = null,
            [new Vector2(-1.8f, 3.6f)] = null,
            [new Vector2(-1.8f, 4.45f)] = null,
            [new Vector2(-1.8f, 5.3f)] = null,
        };
        //right
        Dictionary<Vector2, GameObject> spikesRight = new Dictionary<Vector2, GameObject>()
        {
            [new Vector2(1.8f, 0.2f)] = null,
            [new Vector2(1.8f, 1.05f)] = null,
            [new Vector2(1.8f, 1.9f)] = null,
            [new Vector2(1.8f, 2.75f)] = null,
            [new Vector2(1.8f, 3.6f)] = null,
            [new Vector2(1.8f, 4.45f)] = null,
            [new Vector2(1.8f, 5.3f)] = null,
        };
        //top
        Dictionary<Vector2, GameObject> spikesTop = new Dictionary<Vector2, GameObject>()
        {
            [new Vector2(0f, 5.5f)] = null,
            [new Vector2(-0.85f, 5.5f)] = null,
            [new Vector2(-1.7f, 5.5f)] = null,
            [new Vector2(0.85f, 5.5f)] = null,
            [new Vector2(1.7f, 5.5f)] = null,
        };
        int amountSpikes = 0;
        //left
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            amountSpikes = UnityEngine.Random.Range(0, 8);
            for (int i = 0; i < amountSpikes; i++)
            {
                Vector2 key = GetRandomSpike(spikesLeft);
                spikesLeft[key] = CreateLeftSpikeButton();
                spikesLeft[key].transform.position = key;
            }
        }
        //right
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            amountSpikes = UnityEngine.Random.Range(0, 8);
            for (int i = 0; i < amountSpikes; i++)
            {
                Vector2 key = GetRandomSpike(spikesRight);
                spikesRight[key] = CreateRightSpikeButton();
                spikesRight[key].transform.position = key;
            }
        }
        //top
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            amountSpikes = UnityEngine.Random.Range(0, 8);
            for (int i = 0; i < amountSpikes; i++)
            {
                Vector2 key = GetRandomSpike(spikesTop);
                spikesTop[key] = CreateTopSpikeButton();
                spikesTop[key].transform.position = key;
            }
        }
        //STARS
        GameObject star = CreateStarButton();
        star.transform.position = GetNewPosition();
    }

    private Vector2 GetRandomSpike(Dictionary<Vector2, GameObject> spikes)
    {
        List<Vector2> keys = new List<Vector2>();
        foreach (var item in spikes)
        {
            if (spikes.Values != null)
                keys.Add(item.Key);
        }
        int indexKey = UnityEngine.Random.Range(0, keys.Count);
        return keys[indexKey];
    }

    private Vector3 GetNewPosition()
    {
        float minX = -0.8f;
        float maxX = 0.8f;
        float minY = 0f;
        float maxY = 4.5f;
        float x = UnityEngine.Random.Range(minX, maxX);
        float y = UnityEngine.Random.Range(minY, maxY);
        return new Vector3(x, y);
    }
    #endregion
    [Button(ButtonSizes.Large, Name = "Load Throw")]
    [PropertyOrder(1000)]
    public void LoadThrowButton()
    {
        ThrowConfigData levelData;
        if (_textFile != null)
        {
            levelData = JsonUtility.FromJson<ThrowConfigData>(_textFile.text);
        }
        else
            return;
        //else if(Level == 0)
        //    return;
        //else
        //{
        //    LevelsConfigData levels = LoadLevels();
        //    levelData = levels.Levels.First(x => x.LevelNumber == Level);
        //}
        ThrowData = levelData;
        //LEVEL
        GameObject levelBg = GameObject.Find("background");
        levelBg.GetComponent<SpriteRenderer>().sprite = _settings.SpriteAsset.GetSprite("Background", $"{IdBackgroundSprite}");
        //CANNON
        GameObject cannon = GameObject.Find("Cannon");
        cannon.GetComponent<Cannon>().Initialize(levelData, _settings);
        IdCannonPivot = levelData.Cannon.IdCannonPivot;
        IdCannonMain = levelData.Cannon.IdCannonMain;
        //BASKETS
        if (_basketContainer == null)
            _basketContainer = GameObject.Find("Baskets");
        CleanChildren(_basketContainer.transform);
        _basketMap.Clear();
        BasketsData.Clear();

        foreach (BasketConfigData item in levelData.Baskets)
        {
            TryGetEmptySlotBasket(out BasketColor emptySlot);
            GameObject prefab;
            if (item.BasketType == Basket.BasketType.front)
                prefab = _settings.BasketFrontPrefab;
            else
                prefab = _settings.BasketSidePrefab;

            Basket basket = GameObject.Instantiate(prefab, item.Positions[0], Quaternion.identity, _basketContainer.transform).GetComponent<Basket>();
            if (item.BasketType == Basket.BasketType.side && item.BasketSide == BasketSide.Left)
            {
                basket.transform.rotation = Quaternion.Euler(new Vector3(basket.transform.eulerAngles.x, 0, basket.transform.eulerAngles.z));
                basket.Side = BasketSide.Left;
            }
            else if (item.BasketType == Basket.BasketType.side && item.BasketSide == BasketSide.Right)
            {
                basket.transform.rotation = Quaternion.Euler(new Vector3(basket.transform.eulerAngles.x, 180, basket.transform.eulerAngles.z));
                basket.Side = BasketSide.Right;
            }

            basket.gameObject.GetComponent<GizmosDraw>().GizmosColor = _colorMap[emptySlot];
            for (int i = 1; i < item.Positions.Count - 1; i++)
            {
                GameObject pathPoint = Instantiate(_settings.PointPrefab, item.Positions[i], Quaternion.identity, basket.TrajectoryContainer.transform);
                pathPoint.GetComponent<GizmosDraw>().GizmosColor = _colorMap[emptySlot];
            }

            _basketMap[emptySlot] = basket.gameObject;
            BasketsData[emptySlot] = item;
        }
        //SPIKES
        if (_spikeContainer == null)
            _spikeContainer = GameObject.Find("Spikes");
        CleanChildren(_spikeContainer.transform);

        foreach (SpikeConfigData item in levelData.Spikes)
        {
            GameObject spike = Instantiate(_settings.SpikePrefab, _spikeContainer.transform);
            spike.GetComponent<Spike>().Type = item.Side;
            if (item.Side == BorderType.Left)
                spike.transform.rotation = Quaternion.Euler(_settings.SpikeLeftRotation);
            else if (item.Side == BorderType.Right)
                spike.transform.rotation = Quaternion.Euler(_settings.SpikeRightRotation);
            else if (item.Side == BorderType.Top)
                spike.transform.rotation = Quaternion.Euler(_settings.SpikeTopRotation);

            spike.transform.position = item.Position;
        }
        //STARS
        if (_starContainer == null)
            _starContainer = GameObject.Find("Stars");
        CleanChildren(_starContainer.transform);

        foreach (var item in levelData.Stars)
        {
            Instantiate(_settings.StarPrefab, item.Position, Quaternion.identity, _starContainer.transform);
        }
    }

    //[Button(Name = "Update Level List")]
    [PropertyOrder(1000)]
    public LevelsConfigData LoadLevels()
    {
        var path = GetPath();

        if (!File.Exists(path))
        {
            return null;
        }

        var file = File.ReadAllText(path);
        LevelsConfigData levelsData = JsonUtility.FromJson<LevelsConfigData>(file);
        //LevelsConfigData levelsData = JsonConvert.DeserializeObject<LevelsConfigData>(file);
        if (levelsData == null)
            return null;
        var levelsNumbers = levelsData.Levels.Select(x => x.LevelNumber);
        _existingLevels = levelsNumbers.ToArray();

        return levelsData;
    }

    [Button(ButtonSizes.Large, Name = "Combine Throws")]
    [Group("combinethrows")]
    [PropertyOrder(2050)]
    public void CombineToLevel()
    {
        //SAVE LEVEL TO JSON FILE
        LevelsConfigData levelsData = LoadLevels();
        if (levelsData == null)
            levelsData = new LevelsConfigData();
        LevelData currentLevel = levelsData.Levels.FirstOrDefault(x => x.LevelNumber == LevelNumber);
        LevelData newLevel = new LevelData()
        {
            LevelNumber = LevelNumber,
            idBackgroundSprite = IdBackgroundSprite,
            Throws = new List<ThrowConfigData>()
        };
        foreach (var item in Throws)
        {
            ThrowConfigData currentThrow = JsonUtility.FromJson<ThrowConfigData>(item.text);
            if (currentThrow != null)
                newLevel.Throws.Add(currentThrow);
        }

        if (currentLevel != null)
        {
            levelsData.Levels.Remove(currentLevel);
            levelsData.Levels.Add(newLevel);
        }
        else
        {
            levelsData.Levels.Add(newLevel);
        }
        var path = GetPath();
        var serializedObject = JsonUtility.ToJson(levelsData);
        File.WriteAllText(path, serializedObject);
        //SAVE TO SEP FILE
        var pathSepFile = GetPath($"Level_{LevelNumber}");
        var serializedObjectSepFile = JsonUtility.ToJson(newLevel);
        File.WriteAllText(pathSepFile, serializedObjectSepFile);
        //AssetDatabase.Refresh(); //Comment for build 423, 737
    }

    [Button(ButtonSizes.Large, Name = "Create level file")]
    [PropertyOrder(4000)]
    public void CreateLevelFile()
    {
        LevelsConfigData levelsData = new LevelsConfigData();
        //LevelsConfigData levelsData = LoadLevels();
        foreach (var item in levelsFiles)
        {
            LevelData currentLevel = JsonUtility.FromJson<LevelData>(item.Value.text);
            if (currentLevel != null)
            {
                currentLevel.LevelNumber = item.Key;
                levelsData.Levels.Add(currentLevel);
            }
        }
        var path = GetPath();
        var serializedObject = JsonUtility.ToJson(levelsData);
        File.WriteAllText(path, serializedObject);
    }

    private bool TryGetEmptySlotBasket(out BasketColor color)
    {
        color = BasketColor.Blue;

        foreach (BasketColor item in Enum.GetValues(typeof(BasketColor)))
        {
            if (_basketMap.ContainsKey(item))
                continue;

            color = item;
            return true;
        }

        Debug.LogWarning("No empty basket slot. Delete anyone");
        return false;
    }

    private string GetPath(string fleName = "levelsSaves")
    {
        return $"{Application.dataPath}/_LevelConsctructor/{fleName}.json";
    }
    private void CleanChildren(Transform parent)
    {
        int nbChildren = parent.childCount;
        for (int i = nbChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }

    public enum BasketColor
    {
        Red = 0,
        Green = 1,
        Blue = 2
    }
}

