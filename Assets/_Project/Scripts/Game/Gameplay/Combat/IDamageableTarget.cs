using UnityEngine;

namespace _Project.Game.Gameplay.Combat
{
    public interface IDamageableTarget
    {
        bool IsAlive { get;}
        Vector3 TargetPosition { get;}
        void ApplyDamage(float damage);
    }
}