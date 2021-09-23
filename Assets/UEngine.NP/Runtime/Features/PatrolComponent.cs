using Entitas;
using UnityEngine;

[Game]
public class PatrolComponent : IComponent
{
    public Vector3 Center;
    public float Distance;
    public float Speed;
}