using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UEngine.NP;

[Game][Unique]
public class BehaveTreeDataComponent : IComponent
{
    public Dictionary<string, NP_DataSupportorBase> BehaveTreeDatas;
}