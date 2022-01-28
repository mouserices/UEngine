using Entitas;
using UnityEngine;

[Unit]
public class MoveComponent : IComponent
{
    public bool IsMoving;
    public Vector2 MoveDir;
    public float MoveSpeed;
}