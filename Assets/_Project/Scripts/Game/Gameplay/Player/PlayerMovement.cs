using _Project.Game.Gameplay.Configs;
using _Project.Game.Gameplay.Player.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Game.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _viewTransform;

        private PlayerConfig _playerConfig;
        private IInputManager _inputManager;
        private Vector2 _moveInput;
        private Vector3 _moveDirection;

        public bool IsMoving => _moveDirection.sqrMagnitude > 0.0001f;
        public Transform ViewTransform => _viewTransform != null ? _viewTransform : transform;

        public void Construct(PlayerConfig playerConfig, IInputManager inputManager)
        {
            _playerConfig = playerConfig;
            _inputManager = inputManager;
        }

        private void Reset()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var forward = ViewTransform.forward;
            var right = ViewTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            var moveInput = _inputManager.GetPlayerMovement();
            _moveDirection = (right * moveInput.x + forward * moveInput.y).normalized;

            if (!IsMoving)
            {
                _moveDirection = Vector3.zero;
                return;
            }

            var delta = _moveDirection * (_playerConfig.MoveSpeed * Time.deltaTime);
            _characterController.Move(delta);
        }
    }
}