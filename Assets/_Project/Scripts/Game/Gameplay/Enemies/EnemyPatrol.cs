using _Project.Game.Gameplay.Configs;
using UnityEngine;

namespace _Project.Game.Gameplay.Enemies
{
    public sealed class EnemyPatrol : MonoBehaviour
    {
        private EnemyConfig _enemyConfig;
        private EnemyRoute _route;
        private int _currentPointIndex;
        private int _direction = 1;
        private bool _isActive;

        public void Construct(EnemyConfig enemyConfig)
        {
            _enemyConfig = enemyConfig;
        }

        public void SetRoute(EnemyRoute route, int startPointIndex)
        {
            _route = route;

            if (_route == null || _route.PointsCount == 0)
            {
                Debug.LogWarning("No route defined");
                _currentPointIndex = -1;
                return;
            }

            if (startPointIndex < 0 || startPointIndex >= _route.PointsCount)
            {
                startPointIndex = 0;
            }

            _direction = Random.value < 0.5f ? -1 : 1;
            _currentPointIndex = startPointIndex;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void SnapToCurrentPoint()
        {
            var point = _route.GetPoint(_currentPointIndex);
            transform.position = point.position;
            _currentPointIndex = GetNextPointIndex();
        }

        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            var targetPoint = _route.GetPoint(_currentPointIndex);
            if (targetPoint == null)
            {
                return;
            }

            var currentPosition = transform.position;
            var targetPosition = targetPoint.position;

            var nextPosition = Vector3.MoveTowards(
                currentPosition,
                targetPosition,
                _enemyConfig.MoveSpeed * Time.deltaTime);

            transform.position = nextPosition;

            var lookDirection = targetPosition - currentPosition;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0.0001f)
            {
                transform.forward = lookDirection.normalized;
            }

            var sqrDistance = (targetPosition - nextPosition).sqrMagnitude;
            var tolerance = _enemyConfig.PatrolPointTolerance * _enemyConfig.PatrolPointTolerance;

            if (sqrDistance <= tolerance)
            {
                _currentPointIndex = GetNextPointIndex();
            }
        }

        private int GetNextPointIndex()
        {
            var nextIndex = _currentPointIndex + _direction;

            if (nextIndex >= _route.PointsCount)
            {
                nextIndex = 0;
            }
            else if (nextIndex < 0)
            {
                nextIndex = _route.PointsCount - 1;
            }

            return nextIndex;
        }
    }
}