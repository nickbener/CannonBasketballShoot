using Infrastructure.Providers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MuteOffOn : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private Sprite spriteOff;
    [SerializeField] private Sprite spriteOn;

    private AudioSource mus;
    private PlayerDataProvider _playerDataProvider;

    [Inject]
    private void Inject(PlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;
    }

    private void Start()
    {
        mus = GameObject.Find("BGMusic1").GetComponent<AudioSource>();
        ActiveMusic();
    }

    public void ToggleMute()
    {
        _playerDataProvider.SaveData.IsMusicOff = !_playerDataProvider.SaveData.IsMusicOff;
        _playerDataProvider.SaveData.DemandSave();
        ActiveMusic();
    }

    private void ActiveMusic()
    {
        if (mus == null)
            return;
        mus.mute = _playerDataProvider.SaveData.IsMusicOff;
        if (_playerDataProvider.SaveData.IsMusicOff)
        {
            img.sprite = spriteOff;
        }
        else if (!_playerDataProvider.SaveData.IsMusicOff)
        {
            img.sprite = spriteOn;
        }
    }
}
