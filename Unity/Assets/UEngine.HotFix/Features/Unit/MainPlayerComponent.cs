using Entitas;
using Entitas.CodeGeneration.Attributes;

/// <summary>
/// 标识某个unit是否是主玩家
/// </summary>
[Unit,Unique]
public class MainPlayerComponent : IComponent
{
}