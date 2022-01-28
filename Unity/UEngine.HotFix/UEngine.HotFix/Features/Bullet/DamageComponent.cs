using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unit]
[Cleanup(CleanupMode.DestroyEntity)]
public class DamageComponent : IComponent
{
    public long TargetUnitID;
    public float Damage;
}