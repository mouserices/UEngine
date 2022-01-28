using System.Data.SqlTypes;
using UnityEngine;
// using UnityEngine.InputSystem;

[Service]
public class UnityInputService : IInputService
{
    public Vector2 GetMoveDir()
    {
        Vector2 moveDir = Vector2.zero;
        // if (Keyboard.current.sKey.isPressed) moveDir.y -= 1;
        // if (Keyboard.current.wKey.isPressed) moveDir.y += 1;
        // if (Keyboard.current.aKey.isPressed) moveDir.x -= 1;
        // if (Keyboard.current.dKey.isPressed) moveDir.x += 1;

        return moveDir;
    }

    public bool KeyboardEWawPressed()
    {
        return false;// Keyboard.current.eKey.wasPressedThisFrame;
    }
}