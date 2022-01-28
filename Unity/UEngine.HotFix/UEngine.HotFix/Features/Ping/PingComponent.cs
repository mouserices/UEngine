using Entitas;
using Entitas.CodeGeneration.Attributes;
[Game,Unique]
public class PingComponent : IComponent
{
    public long halfRTT;
}