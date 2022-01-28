using Entitas;
using UnityEngine;

[Unit]
public class PatrolComponent : IComponent
{
    public Vector3 Center;
    public float Distance;
    public float Speed;
}