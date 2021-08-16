using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIConfig
{
    public string NodeComponentName;
    public string UIWindowName;
    public UIType UIType;
    public UIShowMode UIShowMode;
    public UILucenyType UILucenyType;
}

public class UIWindowConfig : SerializedMonoBehaviour
{
    public List<UIConfig> UIConfigs = new List<UIConfig>();
}
