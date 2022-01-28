using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Unit, Cleanup(CleanupMode.DestroyEntity)]
public class InputComponent : IComponent
{
    public KeyCode KeyCode;
}