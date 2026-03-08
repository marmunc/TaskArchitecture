using System.Collections.Generic;
using _Project.Game.Gameplay.Configs;
using _Project.Game.Gameplay.UI;
using UnityEngine;

namespace _Project.Game.Gameplay.Enemies
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyAgent[] _enemyPrefabs;
        [SerializeField] private EnemyRoute[] _availableRoutes;
        [SerializeField] private Transform _enemiesParent;

        private readonly List<EnemyAgent> _spawnedEnemies = new();

        public void Construct(
            EnemyConfig enemyConfig,
            KillCounter killCounter)
        {
            ClearSpawnedEnemies();

            if (_enemyPrefabs == null || _enemyPrefabs.Length == 0)
            {
                Debug.LogWarning($"{nameof(EnemySpawner)}: no enemy prefabs assigned.", this);
                return;
            }

            if (_availableRoutes == null || _availableRoutes.Length == 0)
            {
                Debug.LogWarning($"{nameof(EnemySpawner)}: no routes assigned.", this);
                return;
            }

            var spawnCount = enemyConfig.MaxMobCount;

            for (var i = 0; i < spawnCount; i++)
            {
                var prefab = GetRandomEnemyPrefab();
                var route = GetRandomRoute();

                var startPointIndex = route.GetRandomPointIndex();
                var startPoint = route.GetPoint(startPointIndex);
                if (startPoint == null)
                {
                    continue;
                }

                var enemy = Instantiate(
                    prefab,
                    startPoint.position,
                    startPoint.rotation,
                    _enemiesParent);

                enemy.Construct(
                    enemyConfig,
                    killCounter,
                    route,
                    startPointIndex);

                _spawnedEnemies.Add(enemy);
            }
        }

        private EnemyRoute GetRandomRoute()
        {
            var index = Random.Range(0, _availableRoutes.Length);
            return _availableRoutes[index];
        }

        private EnemyAgent GetRandomEnemyPrefab()
        {
            var index = Random.Range(0, _enemyPrefabs.Length);
            return _enemyPrefabs[index];
        }

        private void ClearSpawnedEnemies()
        {
            for (var i = 0; i < _spawnedEnemies.Count; i++)
            {
                if (_spawnedEnemies[i] != null)
                {
                    Destroy(_spawnedEnemies[i].gameObject);
                }
            }

            _spawnedEnemies.Clear();
        }
    }
}