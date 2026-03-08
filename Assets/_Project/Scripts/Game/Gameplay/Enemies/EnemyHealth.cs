using R3;
using UnityEngine;

namespace _Project.Game.Gameplay.Enemies
{
    public sealed class EnemyHealth
    {
        public ReactiveProperty<float> CurrentHealth { get; }
        public Subject<Unit> Died { get; } = new();

        public bool IsAlive => CurrentHealth.Value > 0f;
        public float MaxHealth { get; }

        public EnemyHealth(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = new ReactiveProperty<float>(maxHealth);
        }

        public void Reset()
        {
            CurrentHealth.Value = MaxHealth;
        }

        public void ApplyDamage(float damage)
        {
            if (!IsAlive)
            {
                return;
            }

            CurrentHealth.Value = Mathf.Max(0f, CurrentHealth.Value - damage);
            Debug.Log($"Current Health: {CurrentHealth.Value}");

            if (CurrentHealth.Value <= 0f)
            {
                Died.OnNext(Unit.Default);
            }
        }
    }
}