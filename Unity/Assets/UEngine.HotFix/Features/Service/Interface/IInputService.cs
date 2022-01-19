using UnityEngine;
public interface IInputService : IService
{
    Vector2 GetMoveDir();
    bool KeyboardEWawPressed();
}

