using Infrastructure.Providers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Sprite _defaultImg;
    [SerializeField] private Sprite _greenImg;
    [SerializeField] private Sprite _blockImg;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private Image[] _stars;
    [SerializeField] private Sprite _starFull;
    [SerializeField] private Sprite _starEmpty;
    [SerializeField] private int _levelNumber;

    private PlayerDataProvider _playerDataProvider;
    [Inject]
    private void Inject(PlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;
    }

    private void Awake()
    {
        if ((_levelNumber == 1 || _playerDataProvider.SaveData.LevelsRecord.ContainsKey(_levelNumber - 1)) && !_playerDataProvider.SaveData.LevelsRecord.ContainsKey(_levelNumber))
        {
            _image.sprite = _greenImg;
            _levelNumberText.gameObject.SetActive(true);
            _levelNumberText.text = _levelNumber.ToString();
            FillStars(0);
            //_button.interactable = true;
        }
        else if (_levelNumber == 1 || _playerDataProvider.SaveData.LevelsRecord.ContainsKey(_levelNumber - 1))
        {
            _image.sprite = _defaultImg;
            _levelNumberText.gameObject.SetActive(true);
            _levelNumberText.text = _levelNumber.ToString();
            FillStars(_playerDataProvider.SaveData.LevelsRecord[_levelNumber]);
        }
        else
        {
            _image.sprite = _blockImg;
            _levelNumberText.gameObject.SetActive(false);
            FillStars(0);
        }
    }

    private void FillStars(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _stars[i].sprite = _starFull;
        }
    }
}
