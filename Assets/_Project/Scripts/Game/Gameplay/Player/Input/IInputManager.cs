using UnityEngine;

namespace _Project.Game.Gameplay.Player.Input
{
    public interface IInputManager
    {
        Vector2 GetPlayerMovement();
        Vector2 GetPlayerLook();
    }
}