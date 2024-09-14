using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TriInspector;
using UnityEngine;
using Utils.Extensions;

namespace UI.Popups
{
    [RequireComponent(typeof(RectTransform))]
    public class Popup : MonoBehaviour
    {
        [SerializeField] protected Vector3 _overShootScale = new(1.2f, 1.2f, 1.2f);
        [SerializeField] protected Vector3 _defaultScale = Vector3.one;
        [SerializeField] protected float _firstTime = 0.5f;
        [SerializeField] protected float _secondTime = 0.2f;
        [SerializeField] protected bool _disableInAwake = true;

        [SerializeField] private RectTransform _rectTransform;

        private bool _isShown;
        private bool _overrideByShow;
        private bool _isInProgress;

        protected virtual void Awake()
        {
            if (!_overrideByShow && _disableInAwake) gameObject.SetActive(false);
        }

        [Button]
        public void Toggle(bool instant = false)
        {
            if (_isShown)
            {
                Hide(instant);
            }
            else
            {
                Show(instant);
            }
        }

        [Button]
        public void Show(bool instant = false)
        {
            if (_isInProgress) return;


            _isShown = true;
            _overrideByShow = true;
            _isInProgress = true;
            if (instant)
            {
                _rectTransform.localScale = _defaultScale;
                gameObject.SetActive(true);
            }
            else
            {
                _rectTransform.localScale = Vector3.zero;
                gameObject.SetActive(true);
                _rectTransform.DOPopOutScale(
                    _overShootScale,
                    _defaultScale,
                    _firstTime,
                    _secondTime,
                    () => _isInProgress = false
                ).SetUpdate(true);
            }
        }

        [Button]
        public async void Hide(bool instant = false)
        {
            if (_isInProgress) return;
            _isShown = false;
            _overrideByShow = true;
            _isInProgress = true;
            if (instant)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _rectTransform.DOPopOutScale(
                    _overShootScale,
                    Vector3.zero,
                    _firstTime,
                    _secondTime,
                    () =>
                    {
                        _isInProgress = false;
                        gameObject.SetActive(false);
                    }).SetUpdate(true);
            }
        }
    }
}