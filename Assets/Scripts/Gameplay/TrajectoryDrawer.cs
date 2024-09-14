using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class TrajectoryDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer = null;
        [SerializeField] [Range(3, 30)] private int _segmentCount = 20;
        [SerializeField] private int _pointCount = default;
        [SerializeField] [Range(0, 100)] private int _showPercentage = default;

        private List<Vector3> _points = new List<Vector3>();

        public void RemoveTrajectory() {

            _lineRenderer.positionCount = default;            
        }

        public void UpdateTrajectory(Rigidbody2D projectile, Vector2 startPoint, Vector2 force) {

            Vector3 velocity = force / projectile.mass * Time.fixedDeltaTime;
            float flightDuration = 2 * velocity.y / Physics2D.gravity.y;
            float stepTime = flightDuration / _segmentCount;

            _points.Clear();

            for (int i = 0; i < _pointCount; i++)
            {

                float stepTimePassed = stepTime * i;

                Vector2 movement = new Vector2()
                {
                    x = velocity.x * stepTimePassed,
                    y = velocity.y * stepTimePassed - 0.5f * Physics2D.gravity.y * Mathf.Pow(stepTimePassed, 2),
                };

                /*if (Physics.Raycast(startPoint, -movement, out RaycastHit2D hit, movement.magnitude))
                {
                    break;
                }*/

                _points.Add(-movement + startPoint);
            }

            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(_points.ToArray());
        }
    }
}