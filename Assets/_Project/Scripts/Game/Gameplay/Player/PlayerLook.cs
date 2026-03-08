using _Project.Game.Gameplay.Configs;
using _Project.Game.Gameplay.Player.Input;
using UnityEngine;

namespace _Project.Game.Gameplay.Player
{
    public sealed class PlayerLook : MonoBehaviour
    {
        [SerializeField] private Transform _playerRoot;
        [SerializeField] private Transform _cameraPivot;

        private IInputManager _inputManager;
        private PlayerConfig _playerConfig;

        private float _pitch;

        public void Construct(PlayerConfig playerConfig, IInputManager inputManager)
        {
            _playerConfig = playerConfig;
            _inputManager = inputManager;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            var lookInput = _inputManager.GetPlayerLook();

            var yawDelta = lookInput.x * _playerConfig.LookSensitivityX * Time.deltaTime;
            var pitchDelta = lookInput.y * _playerConfig.LookSensitivityY * Time.deltaTime;

            _playerRoot.Rotate(0f, yawDelta, 0f);

            _pitch -= pitchDelta;
            _pitch = Mathf.Clamp(_pitch, -_playerConfig.MaxPitchAngle, _playerConfig.MaxPitchAngle);

            _cameraPivot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}