using _Project.Game.Gameplay.Combat;
using Cysharp.Threading.Tasks;
using _Project.Game.Gameplay.Configs;
using _Project.Game.Gameplay.Enemies;
using _Project.Game.Gameplay.Services;
using UnityEngine;

namespace _Project.Game.Gameplay.Player
{
    public sealed class PlayerAutoAttack : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _attackOrigin;
        [SerializeField] private LayerMask _enemyLayerMask;

        [Space]
        [SerializeField] private float _attackRadius = 5f;
        [SerializeField, Range(1f, 180f)] private float _targetingAngle = 75f;
        
        private PlayerConfig _playerConfig;
        private PlayerMovement _playerMovement;
        private TargetSelectionService _targetSelectionService;

        private IDamageableTarget _currentTarget;
        private bool _isAttackLoopRunning;

        public void Construct(
            PlayerConfig playerConfig,
            PlayerMovement playerMovement,
            TargetSelectionService targetSelectionService)
        {
            _playerConfig = playerConfig;
            _playerMovement = playerMovement;
            _targetSelectionService = targetSelectionService;
        }

        private void Update()
        {
            if (_playerMovement.IsMoving)
            {
                _currentTarget = null;
                return;
            }

            _currentTarget = FindBestTarget();

            if (_currentTarget != null && !_isAttackLoopRunning)
            {
                AttackLoopAsync().Forget();
            }
        }

        private IDamageableTarget FindBestTarget()
        {
            var origin = _attackOrigin != null ? _attackOrigin.position : transform.position;
            var forward = _cameraTransform != null ? _cameraTransform.forward : transform.forward;

            return _targetSelectionService.FindNearestVisibleTarget(
                origin,
                forward,
                _attackRadius,
                _targetingAngle,
                _enemyLayerMask);
        }

        private async UniTaskVoid AttackLoopAsync()
        {
            _isAttackLoopRunning = true;

            try
            {
                while (!_playerMovement.IsMoving)
                {
                    var bestTarget = FindBestTarget();

                    if (bestTarget == null)
                    {
                        _currentTarget = null;
                        break;
                    }

                    _currentTarget = bestTarget;
                    _currentTarget.ApplyDamage(_playerConfig.Damage);

                    await UniTask.Delay(
                        Mathf.RoundToInt(_playerConfig.AttackInterval * 1000f),
                        cancellationToken: destroyCancellationToken);
                }
            }
            finally
            {
                _isAttackLoopRunning = false;
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            var origin = _attackOrigin != null ? _attackOrigin.position : transform.position;
            var forwardSource = _cameraTransform != null ? _cameraTransform.forward : transform.forward;

            forwardSource.y = 0f;
            if (forwardSource.sqrMagnitude <= Mathf.Epsilon)
            {
                forwardSource = transform.forward;
                forwardSource.y = 0f;
            }

            if (forwardSource.sqrMagnitude <= Mathf.Epsilon)
            {
                forwardSource = Vector3.forward;
            }

            forwardSource.Normalize();

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(origin, _attackRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, origin + forwardSource * _attackRadius);

            var halfAngle = _targetingAngle * 0.5f;
            var leftBoundary = Quaternion.AngleAxis(-halfAngle, Vector3.up) * forwardSource;
            var rightBoundary = Quaternion.AngleAxis(halfAngle, Vector3.up) * forwardSource;

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(origin, origin + leftBoundary * _attackRadius);
            Gizmos.DrawLine(origin, origin + rightBoundary * _attackRadius);
        }
    }
}