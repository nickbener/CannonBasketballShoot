using Gameplay;
using Infrastructure.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class HeadUpDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _forceTurotial;
        [SerializeField] private Button _shootButton;
        [SerializeField] private Cannon _cannon;
        [SerializeField] private ForceSlider _slider;
        [SerializeField] private TrignometricRotation _trignometricRotation;

        private int _amountClick;
        private PlayerDataProvider _playerDataProvider;

        public int AmountClick => _amountClick;
        [Inject]
        public void Inject(PlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;
        }

        private void OnEnable()
        {
            _shootButton.onClick.AddListener(Shoot);
        }

        private async void Shoot()
        {
            //if(!_playerDataProvider.SaveData.IsTutorialCompleted)
            //{
            //    _cannon.FireIfPossible(_slider.SliderValue);
            //    return;
            //}
            if (UseForceScale())
            {
                _amountClick += 1;
                if (_amountClick == 1)
                {
                    _trignometricRotation.IsStoped = true;
                    _slider.StartMovement();
                    _slider.gameObject.SetActive(true);
                    if(!_playerDataProvider.SaveData.IsTutorialForceCompleted && LevelSettings.SelectedLevel == 5)
                    {
                        _forceTurotial.SetActive(true);
                    }

                }
                else if (_amountClick == 2)
                {
                    _amountClick = 3;
                    _cannon.FireIfPossible(_slider.SliderValue);
                    await Task.Delay(1000);
                    _slider.gameObject.SetActive(false);
                    _trignometricRotation.IsStoped = false;
                    _amountClick = 0;
                }
            }
            else
            {
                _amountClick = 0;
                _cannon.FireIfPossible(_slider.SliderValue);
            }
        }

        private bool UseForceScale()
        {
            int[] levelsNoSlider = { 1, 2, 3, 4 };
            if (levelsNoSlider.Contains(LevelSettings.SelectedLevel))
                return false;
            else
                return true;
        }

        private void OnDisable()
        {
            _shootButton.onClick.RemoveListener(Shoot);
        }

        public void Scene(int num)
        {
            SceneManager.LoadScene(num);
        }
    }
}