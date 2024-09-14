using UI.Popups;
using UnityEngine;
using DG.Tweening;
using Utils.Extensions;

public class WordPopup : Popup
{
    Sequence _sequence;
    public void ShowWordPopup()
    {
        _sequence?.Kill();
        _sequence = DOTween.Sequence();
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        _sequence.Append(rectTransform.DOPopOutScale(
            _overShootScale,
            _defaultScale,
            _firstTime,
            _secondTime
        ));

        _sequence.Append(rectTransform.DOPopOutScale(
                    _overShootScale,
                    Vector3.zero,
                    _firstTime,
                    _secondTime,
                    () =>
                    {
                        gameObject.SetActive(false);
                    }));

        //_sequence.Play();
    }
}
