using _Project.Game.Gameplay.Configs;
using _Project.Game.Gameplay.UI;
using UnityEngine;

namespace _Project.Game.Gameplay.Enemies
{
    public sealed class EnemyFactory
    {
        private readonly EnemyConfig _enemyConfig;
        private readonly KillCounter _killCounter;

        public EnemyFactory(EnemyConfig enemyConfig, KillCounter killCounter)
        {
            _enemyConfig = enemyConfig;
            _killCounter = killCounter;
        }

        public EnemyAgent Create(
            EnemyAgent prefab,
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            EnemyRoute route,
            int startPointIndex)
        {
            var enemy = Object.Instantiate(prefab, position, rotation, parent);
            enemy.Construct(_enemyConfig, _killCounter, route, startPointIndex);
            return enemy;
        }
    }
}