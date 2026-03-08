using _Project.Game.Gameplay.Configs;
using _Project.Game.Gameplay.Player.Input;
using _Project.Game.Gameplay.Services;
using UnityEngine;

namespace _Project.Game.Gameplay.Player
{
    public sealed class PlayerRoot : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerLook _look;
        [SerializeField] private PlayerAutoAttack _autoAttack;

        public void Construct(PlayerConfig playerConfig, IInputManager inputManager, TargetSelectionService targetSelectionService)
        {
            _movement.Construct(playerConfig, inputManager);
            _look.Construct(playerConfig, inputManager);
            _autoAttack.Construct(playerConfig, _movement, targetSelectionService);
        }
    }
}