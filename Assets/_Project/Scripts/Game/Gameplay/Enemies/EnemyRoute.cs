using UnityEngine;

namespace _Project.Game.Gameplay.Enemies
{
    public sealed class EnemyRoute : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;

        public int PointsCount => _points != null ? _points.Length : 0;

        public Transform GetPoint(int index)
        {
            if (PointsCount == 0)
            {
                Debug.LogWarning("No points available");
                return null;
            }

            if (index < 0 || index >= _points.Length)
            {
                return null;
            }

            return _points[index];
        }

        public int GetRandomPointIndex()
        {
            if (PointsCount == 0)
            {
                Debug.LogWarning("No points available");
                return -1;
            }

            return Random.Range(0, _points.Length);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_points == null || _points.Length == 0)
            {
                return;
            }

            Gizmos.color = Color.magenta;

            for (var i = 0; i < _points.Length; i++)
            {
                var point = _points[i];
                if (point == null)
                {
                    continue;
                }

                Gizmos.DrawSphere(point.position, 0.15f);

                var nextIndex = (i + 1) % _points.Length;
                var nextPoint = _points[nextIndex];

                if (nextPoint != null)
                {
                    Gizmos.DrawLine(point.position, nextPoint.position);
                }
            }
        }
#endif
    }
}