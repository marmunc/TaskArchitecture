using UnityEngine;

namespace _Project.Game.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Gameplay/PlayerConfig")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [field: SerializeField, Min(0f)] public float MoveSpeed { get; private set; } = 5f;
        [field: SerializeField, Min(0.1f)] public float AttackInterval { get; private set; } = 0.5f;
        [field: SerializeField, Min(0f)] public float Damage { get; private set; } = 1f;
        [field: SerializeField, Min(0f)] public float AttackRadius { get; private set; } = 5f;
        [field: SerializeField, Range(1f, 180f)] public float TargetingAngle { get; private set; } = 75f;
        
        [field: Space]
        [field: SerializeField, Min(0.01f)] public float LookSensitivityX  { get; private set; } = 150f;
        [field: SerializeField, Min(0.01f)] public float LookSensitivityY { get; private set; } = 150f;
        [field: SerializeField, Range(1f, 89f)] public float MaxPitchAngle { get; private set; } = 80f;
    }
}