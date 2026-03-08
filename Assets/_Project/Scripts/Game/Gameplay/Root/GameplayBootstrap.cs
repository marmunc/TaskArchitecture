using _Project.Game.Gameplay.Configs;
using _Project.Game.Gameplay.Enemies;
using _Project.Game.Gameplay.Player;
using _Project.Game.Gameplay.Player.Input;
using _Project.Game.Gameplay.Services;
using _Project.Game.Gameplay.UI;
using BaCon;
using UnityEngine;

namespace _Project.Game.Gameplay.Root
{
    public sealed class GameplayBootstrap : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;

        [Header("Scene Roots")]
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private PlayerRoot _playerRoot;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private KillCounterView _killCounterView;

        private DIContainer _container;

        private void Awake()
        {
            BuildContainer();
            BindSceneObjects();
        }

        private void OnDestroy()
        {
            _container?.Dispose();
        }

        private void BuildContainer()
        {
            _container = new DIContainer();

            _container.RegisterFactory(_ => new TargetSelectionService())
                .AsSingle();

            _container.RegisterFactory(_ => new KillCounter())
                .AsSingle();
            
            _container.RegisterFactory(c =>
                    new EnemyFactory(
                        _enemyConfig,
                        c.Resolve<KillCounter>()))
                .AsSingle();
        }

        private void BindSceneObjects()
        {
            _playerRoot.Construct(
                _playerConfig,
                _inputManager,
                _container.Resolve<TargetSelectionService>());
            
            _enemySpawner.Construct(
                _container.Resolve<EnemyFactory>(),
                _enemyConfig.MaxMobCount);

            _enemySpawner.Spawn();

            _killCounterView.Construct(
                _container.Resolve<KillCounter>());
        }
    }
}