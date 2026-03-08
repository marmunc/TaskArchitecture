using Cysharp.Threading.Tasks;
using R3;
using _Project.Game.Gameplay.Combat;
using _Project.Game.Gameplay.Configs;
using _Project.Game.Gameplay.UI;
using UnityEngine;

namespace _Project.Game.Gameplay.Enemies
{
    public sealed class EnemyAgent : MonoBehaviour, IDamageableTarget
    {
        [SerializeField] private EnemyPatrol _enemyPatrol;
        [SerializeField] private Collider _bodyCollider;
        [SerializeField] private GameObject _visualRoot;

        private EnemyConfig _enemyConfig;
        private KillCounter _killCounter;
        private EnemyHealth _enemyHealth;
        private CompositeDisposable _disposables;

        private EnemyRoute _assignedRoute;
        private int _startPointIndex;
        private bool _isRespawning;

        public bool IsAlive => _enemyHealth != null && _enemyHealth.IsAlive;
        public Vector3 TargetPosition => transform.position;

        public void Construct(
            EnemyConfig enemyConfig,
            KillCounter killCounter,
            EnemyRoute route,
            int startPointIndex)
        {
            _enemyConfig = enemyConfig;
            _killCounter = killCounter;
            _assignedRoute = route;
            _startPointIndex = startPointIndex;

            _enemyHealth = new EnemyHealth(_enemyConfig.MaxHealth);
            _enemyPatrol.Construct(_enemyConfig);
            _enemyPatrol.SetRoute(_assignedRoute, _startPointIndex);

            _disposables = new CompositeDisposable();

            _enemyHealth.Died
                .Subscribe(_ => HandleDeath())
                .AddTo(_disposables);

            Activate();
        }

        public void ApplyDamage(float damage)
        {
            if (!IsAlive)
            {
                return;
            }

            _enemyHealth.ApplyDamage(damage);
        }

        private void HandleDeath()
        {
            if (_isRespawning)
            {
                return;
            }

            _killCounter.RegisterKill();
            Deactivate();
            RespawnAsync().Forget();
        }

        private void Activate()
        {
            _isRespawning = false;

            _enemyPatrol.SetRoute(_assignedRoute, _startPointIndex);
            _enemyPatrol.SnapToCurrentPoint();

            _enemyHealth.Reset();

            if (_visualRoot != null)
            {
                _visualRoot.SetActive(true);
            }

            if (_bodyCollider != null)
            {
                _bodyCollider.enabled = true;
            }

            _enemyPatrol.SetActive(true);
        }

        private void Deactivate()
        {
            _enemyPatrol.SetActive(false);

            if (_bodyCollider != null)
            {
                _bodyCollider.enabled = false;
            }

            if (_visualRoot != null)
            {
                _visualRoot.SetActive(false);
            }
        }

        private async UniTaskVoid RespawnAsync()
        {
            _isRespawning = true;

            await UniTask.Delay(
                Mathf.RoundToInt(_enemyConfig.RespawnDelay * 1000f),
                cancellationToken: destroyCancellationToken);

            Activate();
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}