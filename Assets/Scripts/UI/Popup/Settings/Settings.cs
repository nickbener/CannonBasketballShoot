using Infrastructure.Providers;
using Management.Roots;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Zenject;

public class Settings : MonoBehaviour
{
    [SerializeField] private Popup _settingPopup;
    [SerializeField] private Popup _loginPopup;
    [SerializeField] private Popup _RatingPopup;
    [SerializeField] private Popup _RatingReportPopup;
    [SerializeField] private Popup _ReportPopup;
    [SerializeField] private Popup _SurePopup;
    [SerializeField] private Popup _SureDeleteSavesPopup;

    [SerializeField] private GameplaySceneRoot _gameplaySceneRoot;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _loginButton;
    [SerializeField] private GameObject _editButton;
    [SerializeField] private TextMeshProUGUI _currentNick;
    //Login
    [Header("Login")]
    [SerializeField] private TMP_InputField _inputName;
    [SerializeField] private GameObject _saveButton;
    [SerializeField] private GameObject _editingButtons;
    [SerializeField] private Image _currentAvatar;
    [SerializeField] private GameObject _selectAvatarButton;
    [SerializeField] private GameObject _avatarScroller;
    [SerializeField] private Transform _avatars;
    //Ratng
    [Header("Ratng")]
    [SerializeField] private GameObject _rateButton;
    [SerializeField] private GameObject _ratedPanel;
    [SerializeField] private Image[] _stars;
    [SerializeField] private Sprite _fillStar;
    [SerializeField] private Sprite _emptyStar;
    [SerializeField] private Image[] _stars2;
    [SerializeField] private TMP_InputField _inputRating;
    //Rate
    [Header("Rate")]
    [SerializeField] private TMP_InputField _reportRating;


    private PlayerDataProvider _playerDataProvider;
    private ResourceSystemService _resourceSystemService;
    private Popup _lastPopup;
    private int _exitScene;
    private int _amountStar;

    private int _selectedAvatar;

    [Inject]
    private void Inject(PlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;
        _selectedAvatar = _playerDataProvider.SaveData.IdAvatar;
    }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == ScenesMetadata.GameplaySceneName)
            _restartButton.SetActive(true);
        else
            _restartButton.SetActive(false);

        //Initialize();
    }

    public void Initialize(ResourceSystemService resourceSystemService)
    {
        _resourceSystemService = resourceSystemService;
        UpdateView();
    }

    private void UpdateView()
    {
        _currentNick.text = _playerDataProvider.SaveData.Nickname;
        if (_playerDataProvider.SaveData.IdAvatar == -1)
        {
            _loginButton.SetActive(true);
            _editButton.SetActive(false);
        }
        else
        {
            _loginButton.SetActive(false);
            _editButton.SetActive(true);
        }


        if (_playerDataProvider.SaveData.IsRated)
        {
            _ratedPanel.SetActive(true);
            _rateButton.SetActive(false);
        }
        else
        {
            _ratedPanel.SetActive(false);
            _rateButton.SetActive(true);
        }
    }

    public void ShowSettings()
    {
        _settingPopup.Show();
    }

    public void SecondPanelClose(Popup popup)
    {
        popup.Hide();
        _settingPopup.gameObject.SetActive(true);
    }

    public void SecondPanelShow(Popup popup)
    {
        _settingPopup.gameObject.SetActive(false);
        popup.Show();
        //popup.gameObject.SetActive(true);
        if (popup == _loginPopup)
        {
            UpdateLoginPopup();
        }
    }

    public void AcceptPanelCancel()
    {
        _SurePopup.Hide();
        _lastPopup.gameObject.SetActive(true);
    }

    public void AcceptPanelAccept()
    {
        _SurePopup.Hide();
        if (SceneManager.GetActiveScene().name == ScenesMetadata.GameplaySceneName && _gameplaySceneRoot != null)
            _gameplaySceneRoot.Dispose();
        SceneManager.LoadScene(_exitScene);
    }

    public void ExitButton()
    {
        _lastPopup = _settingPopup;
        if (SceneManager.GetActiveScene().name == ScenesMetadata.GameplaySceneName)
            _exitScene = 6;
        else
            _exitScene = 0;
        _settingPopup.gameObject.SetActive(false);
        _SurePopup.Show();
        //_SurePopup.gameObject.SetActive(true);
    }

    public void RestartButton()
    {
        _exitScene = 4;
        _settingPopup.gameObject.SetActive(false);
        _SurePopup.Show();
        //_SurePopup.gameObject.SetActive(true);
    }
    public void OpenAvatarButton()
    {
        _selectAvatarButton.gameObject.SetActive(false);
        _avatarScroller.gameObject.SetActive(true);
    }
    public void SelectAvatarButton(int index)
    {
        _selectedAvatar = index;
        UpdateLoginPopup();
    }
    public void SaveButton()
    {
        _playerDataProvider.SaveData.IdAvatar = _selectedAvatar;
        _playerDataProvider.SaveData.Nickname = _inputName.text;
        _playerDataProvider.SaveData.DemandSave();

        _currentNick.text = _playerDataProvider.SaveData.Nickname;
        _loginButton.SetActive(false);
        _editButton.SetActive(true);
        //UpdateLoginPopup();
    }

    public void LogoutButton()
    {
        _lastPopup = _loginPopup;
        _loginPopup.gameObject.SetActive(false);
        _SureDeleteSavesPopup.Show();
        //_SureDeleteSavesPopup.gameObject.SetActive(true);
    }

    public void LogoutAcceptButton()
    {
        //DeleteSaves
        string path = Path.Combine(Application.persistentDataPath, "Data/Models");
        DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }
        _playerDataProvider.SaveData.Nickname = "";
        _playerDataProvider.SaveData.IdAvatar = -1;
        _playerDataProvider.SaveData.IsTutorialBusterCompleted = false;
        _playerDataProvider.SaveData.IsTutorialCompleted = false;
        _playerDataProvider.SaveData.LevelsRecord = new();

        _resourceSystemService.SetResourceAmount(ResourceType.Gold, 0);
        _resourceSystemService.SetResourceAmount(ResourceType.Star, 0);
        _resourceSystemService.SetResourceAmount(ResourceType.Score, 0);
        //_resourceSystemService.SetResourceAmount(ResourceType.BustTime, 0);
        //_resourceSystemService.SetResourceAmount(ResourceType.BustDef, 0);
        //_resourceSystemService.SetResourceAmount(ResourceType.BustCannon, 0);
        if (SceneManager.GetActiveScene().name == ScenesMetadata.GameplaySceneName && _gameplaySceneRoot != null)
            _gameplaySceneRoot.Dispose();
        SceneManager.LoadScene(0);
    }

    public void ClickStar(int position)
    {
        foreach (var item in _stars)
        {
            item.sprite = _emptyStar;
        }

        for (int i = 0; i < position; i++)
        {
            _stars[i].sprite = _fillStar;
        }
        _amountStar = position;
    }

    public void RateButton()
    {
        if (_amountStar > 3)
        {
            //Open Google play
            Application.OpenURL("market://details?id=com.gamezmonster.cannonbasketball");
            _RatingPopup.Hide();
        }
        else
        {
            _RatingPopup.gameObject.SetActive(false);
            _RatingReportPopup.Show();
            //_RatingReportPopup.gameObject.SetActive(true);

            foreach (var item in _stars2)
            {
                item.sprite = _emptyStar;
            }

            for (int i = 0; i < _amountStar; i++)
            {
                _stars2[i].sprite = _fillStar;
            }
        }
        _playerDataProvider.SaveData.IsRated = true;
        UpdateView();
    }

    public void SendRating()
    {
        _RatingReportPopup.Hide();
        _settingPopup.gameObject.SetActive(true);
    }

    public void CloseRatingPanel()
    {
        _RatingReportPopup.Hide();
        _RatingPopup.gameObject.SetActive(true);
    }

    public void SendReportButton()
    {
        //_reportRating.text
        SecondPanelClose(_ReportPopup);
    }

    private void UpdateLoginPopup()
    {
        _currentNick.text = _playerDataProvider.SaveData.Nickname;
        _selectAvatarButton.gameObject.SetActive(true);
        _avatarScroller.gameObject.SetActive(false);
        if (_selectedAvatar == -1)
        {
            _saveButton.SetActive(true);
            _editingButtons.SetActive(false);
        }
        else
        {
            Transform avatar = _avatars.transform.GetChild(_selectedAvatar);
            if (avatar != null)
                _currentAvatar.sprite = avatar.gameObject.GetComponent<Image>().sprite;
            _saveButton.SetActive(false);
            _editingButtons.SetActive(true);
        }
    }
}
