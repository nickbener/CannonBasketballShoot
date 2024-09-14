using System;
using System.Collections.Generic;
using CodeBase;
using Gameplay.Environment;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class BallsBuffer : MonoBehaviour
    {
        [SerializeField] private List<Transform> _ballPositions;
        [SerializeField] private List<Ball> _initialBalls;
        [SerializeField] private TextMeshPro _ballsCountTextMesh;
        
        private Queue<Ball> _balls;

        public int MaxSize => _ballPositions.Count;
        public int BallsCount => _balls.Count;
        
        private void Awake()
        {
            _balls = new Queue<Ball>(_initialBalls);
            UpdatePlacements();
        }

        public bool TryPeek(out Ball peekedBall)
        {
            if (_balls.Count > 0)
            {
                peekedBall = _balls.Peek();
                return true;
            }
            else
            {
                peekedBall = null;
                return false;
            }
        }

        public bool TryEnqueue(Ball ball)
        {
            if (MaxSize > BallsCount)
            {
                ball.transform.SetParent(transform, true);
                _balls.Enqueue(ball);
                UpdatePlacements();
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool TryDequeue(out Ball ball)
        {
            if (_balls.Count > 0)
            {
                ball = _balls.Dequeue();
                UpdatePlacements();
                return true;
            }
            else
            {
                ball = null;
                return false;
            }
        }

        private void UpdatePlacements()
        {
            int positionIndex = 0;
            
            foreach (Ball ball in _balls)
            {
                if (positionIndex >= _ballPositions.Count)
                {
                    break;
                }

                ball.transform.position = _ballPositions[positionIndex].position;
                positionIndex++;
            }
            
            _ballsCountTextMesh.SetText($"{BallsCount}/{MaxSize}");
        }

    }
}