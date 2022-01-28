using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UEngine.NP;

[Game][Unique]
public class BehaveTreeDataComponent : IComponent
{
    public Dictionary<int, NP_DataSupportorBase> NameToBehaveTreeDatas;
}