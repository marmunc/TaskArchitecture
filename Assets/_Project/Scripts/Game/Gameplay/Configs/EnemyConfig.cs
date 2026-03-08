using UnityEngine;

namespace _Project.Game.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Gameplay/EnemyConfig")]
    public sealed class EnemyConfig : ScriptableObject
    {
        [field: SerializeField, Min(1f)] public float MaxHealth { get; private set; } = 3f;
        [field: SerializeField, Min(0.1f)] public float RespawnDelay { get; private set; } = 3f;
        [field: SerializeField, Min(1)] public int MaxMobCount { get; private set; } = 5;
        [field: SerializeField, Min(0f)] public float MoveSpeed { get; private set; } = 2f;
        [field: SerializeField, Min(0.01f)] public float PatrolPointTolerance { get; private set; } = 0.15f;
    }
}