using _Project.Game.Gameplay.Combat;
using UnityEngine;

namespace _Project.Game.Gameplay.Enemies
{
    public sealed class EnemyAgent : MonoBehaviour, IDamageableTarget
    {
        [SerializeField] private float _health = 50f;

        public Transform TargetTransform => transform;

        public bool IsAlive => _health > 0;
        public Vector3 TargetPosition => transform.position;

        public void ApplyDamage(float damage)
        {
            if (!IsAlive)
            {
                return;
            }

            _health = Mathf.Max(0f, _health - damage);
            Debug.Log($"{name} took {damage} damage. Current HP: {_health}", this);
        }
    }
}