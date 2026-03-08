using UnityEngine;

namespace _Project.Game.Gameplay.Player.Input
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        private PlayerControls _playerControls;

        private void Awake()
        {
            _playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
        }

        public Vector2 GetPlayerMovement()
        {
            return _playerControls.Player.Move.ReadValue<Vector2>();
        }

        public Vector2 GetPlayerLook()
        {
            return _playerControls.Player.Look.ReadValue<Vector2>();
        }
    }
}