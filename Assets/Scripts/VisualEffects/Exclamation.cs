using DG.Tweening;
using TMPro;
using TriInspector;
using UnityEngine;

namespace VisualEffects
{
    public class Exclamation : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _textMesh;
        [SerializeField] private Transform _motionTarget;
        
        private Sequence _tween;

        private Vector3 _pos;
        
        private void Awake()
        {
            transform.localScale = Vector3.zero;
            _pos = transform.localPosition;
        }

        [Button]
        private void TestRun()
        {
            _tween.Kill();
            transform.localScale = Vector3.zero;
            transform.localPosition = _pos;
            _textMesh.alpha = 1.0f;
            Run("Goal!");
        }
        
        public Exclamation Run(string text)
        {
            _textMesh.SetText(text);

            _tween = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack))
                .Join(transform.DOLocalMove(_motionTarget.localPosition, 2.0f).SetEase(Ease.OutSine))
                .Join(_textMesh.DOFade(0.0f, 2.0f).SetEase(Ease.InQuint))
                .AppendCallback(SelfDestroy)
                .SetLink(gameObject);

            return this;
        }

        private void SelfDestroy()
        {
            Destroy(gameObject);
        }

    }
}