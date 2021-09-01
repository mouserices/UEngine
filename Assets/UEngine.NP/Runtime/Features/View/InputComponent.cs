using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Cleanup(CleanupMode.DestroyEntity)]
public class InputComponent : IComponent
{
    public KeyCode KeyCode;
}