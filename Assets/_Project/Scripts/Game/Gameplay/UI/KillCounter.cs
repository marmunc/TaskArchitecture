using R3;
using UnityEngine;

namespace _Project.Game.Gameplay.UI
{
    public sealed class KillCounter
    {
        public ReactiveProperty<int> Kills { get; } = new(0);

        public void RegisterKill()
        {
            Kills.Value++;
            Debug.Log($"Registered Kill: {Kills.Value}");
        }
    }
}