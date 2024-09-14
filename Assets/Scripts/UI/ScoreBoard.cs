using System;
using CodeBase;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreBoard : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _scoreTextMesh;
        
        public void UpdateView(int score)
        {
            _scoreTextMesh.SetText(score.ToString());
        }
    }
}