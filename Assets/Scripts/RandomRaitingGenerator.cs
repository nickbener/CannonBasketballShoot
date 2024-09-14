using Infrastructure.Providers;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class RandomRaitingGenerator : MonoBehaviour
{
    [SerializeField] private RatingType _resourceType; // тип рейтинга
    [SerializeField] private Sprite[] _avatars; // avatars
    public GameObject prefab; // Префаб, который будем спаунить
    public Transform container; // Объект Container, в котором будем создавать префабы
    public Text[] topTexts; // Текстовые поля для топ-3 мест
    public Image[] topAvatars; // Изображения для топ-3 мест
    public Text[] topNick; // Никнейм для топ-3 мест

    // Массивы аватарок и никнеймов
    public string[] nicknames;

    private List<string> availableNicknames; // Список доступных никнеймов

    private ResourceSystemService _resourceSystemService;
    private PlayerDataProvider _playerDataProvider;
    Dictionary<RandomRaitingGenerator.RatingType, int> _scoreAmount;
    private int _indexAvatar;
    private void Start()
    {

    }

    public void Initialize(PlayerDataProvider playerDataProvider, ResourceSystemService resourceService, Dictionary<RandomRaitingGenerator.RatingType, int> scoreAmount)
    {
        _resourceSystemService = resourceService;
        _playerDataProvider = playerDataProvider;
        _scoreAmount = scoreAmount;
        _indexAvatar = _playerDataProvider.SaveData.IdAvatar;
        if (_indexAvatar == -1)
            _indexAvatar = 0;
        BuildRating();
    }

    private void BuildRating()
    {
        availableNicknames = new List<string>(nicknames); // Инициализируем список доступных никнеймов
        int minValue = GetMinValue();
        int maxValue = GetMaxValue();
        // Генерируем 12 рандомных чисел от 1 до 100 и сортируем их по убыванию
        int[] scores = new int[12];
        for (int i = 0; i < 12; i++)
        {
            scores[i] = Random.Range(minValue, maxValue);
        }
        scores[11] = _scoreAmount[_resourceType];//Добавляем игрока в массив
        System.Array.Sort(scores);
        System.Array.Reverse(scores);
        int playerIndex = GetPlayerIndex(scores); //Получаем индекс игрока
        // Выбираем случайные аватарки и никнеймы для топ-3 мест
        for (int i = 0; i < 3; i++)
        {
            if (i == playerIndex)// Устанавливаем данные игрока
            {
                topTexts[i].text = scores[i].ToString();
                topNick[i].text = $"{_playerDataProvider.SaveData.Nickname} (you)";
                topAvatars[i].sprite = _avatars[_indexAvatar];
                continue;
            }
            //int randomIndex = Random.Range(0, avatars.Length);
            topTexts[i].text = scores[i].ToString(); // Устанавливаем тексты для топ-3 мест
            topNick[i].text = availableNicknames[i];
            //topAvatarsArray[i] = avatars[randomIndex]; // Сохраняем случайные аватарки для топ-3 мест
            topAvatars[i].sprite = GetRandomAvatar();// topAvatarsArray[i]; // Устанавливаем изображения для топ-3 мест
            //avatars[randomIndex] = avatars[avatars.Length - 1]; // Заменяем выбранную аватарку на последнюю в массиве
            //avatars = ResizeArray(avatars, avatars.Length - 1); // Уменьшаем размер массива аватарок
        }
        GameObject newPrefab;
        RaitingPrefabScript playerPrefab;
        // Создаем префабы согласно сгенерированным данным, начиная с 4 позиции
        for (int i = 3; i < 12; i++)
        {
            if (i == playerIndex) // Устанавливаем данные игрока
            {
                newPrefab = Instantiate(prefab, container);
                playerPrefab = newPrefab.GetComponent<RaitingPrefabScript>();
                playerPrefab.score = scores[i];
                playerPrefab.position = i + 1;
                playerPrefab.avatar = _avatars[_indexAvatar]; ;
                playerPrefab.nickname = $"{_playerDataProvider.SaveData.Nickname} (you)";
                continue;
            }
            newPrefab = Instantiate(prefab, container);
            playerPrefab = newPrefab.GetComponent<RaitingPrefabScript>();
            playerPrefab.score = scores[i];
            playerPrefab.position = i + 1; // Начиная с позиции 4

            // Выбираем случайную доступную аватарку
            //int randomAvatarIndex = Random.Range(0, availableAvatars.Count);
            playerPrefab.avatar = GetRandomAvatar();// availableAvatars[randomAvatarIndex];
            //availableAvatars.RemoveAt(randomAvatarIndex); // Удаляем использованную аватарку из списка доступных

            // Выбираем случайный никнейм из доступных
            int randomNicknameIndex = Random.Range(0, availableNicknames.Count);
            playerPrefab.nickname = availableNicknames[randomNicknameIndex];
            availableNicknames.RemoveAt(randomNicknameIndex); // Удаляем использованный никнейм из списка доступных
        }

        // Метод для изменения размера массива
        //T[] ResizeArray<T>(T[] array, int newSize)
        //{
        //    T[] newArray = new T[newSize];
        //    for (int i = 0; i < Mathf.Min(array.Length, newSize); i++)
        //    {
        //        newArray[i] = array[i];
        //    }
        //    return newArray;
        //}
    }

    private Sprite GetRandomAvatar()
    {
        int index = Random.Range(0, _avatars.Length);
        return _avatars[index];
    }

    private int GetPlayerIndex(int[] scores)
    {
        int index = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == _scoreAmount[_resourceType])
            {
                index = i;
                break;
            }
        }
        return index;
    }

    private int GetMinValue()
    {
        int value = 0;
        int range = 0;
        switch (_resourceType)
        {
            case RatingType.level:
                range = 5;
                break;
            case RatingType.star:
                range = 50;
                break;
            case RatingType.score:
                range = 50;
                break;
            default:
                break;
        }
        value = _scoreAmount[_resourceType] - range < 1 ? 1 : _scoreAmount[_resourceType] - range;
        return value;
    }
    private int GetMaxValue()
    {
        int value = 0;
        switch (_resourceType)
        {
            case RatingType.level:
                value = _scoreAmount[_resourceType] + 5 > 10 ? 10 : _scoreAmount[_resourceType] + 5;
                break;
            case RatingType.star:
                value = _scoreAmount[_resourceType] + 50;
                break;
            case RatingType.score:
                value = _scoreAmount[_resourceType] + 50;
                break;
            default:
                break;
        }
        return value;
    }

    public enum RatingType
    {
        star,
        score,
        level
    }
}