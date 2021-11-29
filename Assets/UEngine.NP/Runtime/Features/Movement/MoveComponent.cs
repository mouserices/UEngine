using Entitas;
using UnityEngine;

[Game]
public class MoveComponent : IComponent
{
    public bool IsMoving;
    public Vector2 MoveDir;
    public float MoveSpeed;
}