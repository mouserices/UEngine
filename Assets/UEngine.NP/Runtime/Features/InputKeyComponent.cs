using System.Collections.Generic;
using Entitas;
using UnityEngine;

[Game]
public class InputKeyComponent : IComponent
{
    public Dictionary<KeyCode, int> KeyCodes;
}