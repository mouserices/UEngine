using Entitas;

[Buff]
public class BuffComponent : IComponent
{
    public long BuffID;
    public long SourceUnitID;
    public int SourceSkillID;
    public string BehavetreeName;
}