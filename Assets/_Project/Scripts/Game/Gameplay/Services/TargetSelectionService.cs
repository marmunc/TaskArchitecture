using _Project.Game.Gameplay.Combat;
using _Project.Game.Gameplay.Enemies;
using UnityEngine;

namespace _Project.Game.Gameplay.Services
{
    public sealed class TargetSelectionService
    {
        private readonly Collider[] _overlapResults;

        public TargetSelectionService(int maxCollidersCount = 32)
        {
            _overlapResults = new Collider[maxCollidersCount];
        }

        public IDamageableTarget FindNearestVisibleTarget(
            Vector3 origin,
            Vector3 forward,
            float attackRadius,
            float targetingAngle,
            LayerMask enemyLayerMask)
        {
            forward.y = 0f;

            if (forward.sqrMagnitude <= Mathf.Epsilon)
            {
                return null;
            }

            forward.Normalize();

            var hitsCount = Physics.OverlapSphereNonAlloc(
                origin,
                attackRadius,
                _overlapResults,
                enemyLayerMask,
                QueryTriggerInteraction.Ignore);

            IDamageableTarget bestTarget = null;
            var bestSqrDistance = float.MaxValue;
            var halfAngle = targetingAngle * 0.5f;

            for (var i = 0; i < hitsCount; i++)
            {
                var hitCollider = _overlapResults[i];
                if (hitCollider == null)
                {
                    continue;
                }

                if (!hitCollider.TryGetComponent(out IDamageableTarget enemy))
                {
                    continue;
                }

                if (!enemy.IsAlive)
                {
                    continue;
                }

                var direction = enemy.TargetPosition - origin;
                direction.y = 0f;

                var sqrDistance = direction.sqrMagnitude;
                if (sqrDistance <= Mathf.Epsilon)
                {
                    continue;
                }

                var angle = Vector3.Angle(forward, direction.normalized);
                if (angle > halfAngle)
                {
                    continue;
                }

                if (sqrDistance < bestSqrDistance)
                {
                    bestSqrDistance = sqrDistance;
                    bestTarget = enemy;
                }
            }

            ClearResults(hitsCount);

            return bestTarget;
        }

        private void ClearResults(int hitsCount)
        {
            for (var i = 0; i < hitsCount; i++)
            {
                _overlapResults[i] = null;
            }
        }
    }
}