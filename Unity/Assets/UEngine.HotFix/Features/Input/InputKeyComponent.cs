using System.Collections.Generic;
using Entitas;
using UnityEngine;

[Unit]
public class InputKeyComponent : IComponent
{
    public Dictionary<KeyCode, int> KeyCodes;
}