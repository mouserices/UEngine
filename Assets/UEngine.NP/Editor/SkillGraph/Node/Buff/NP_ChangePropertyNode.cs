using GraphProcessor;
using Sirenix.OdinInspector;

[NodeMenuItem("NPBehave行为树/Buff/更改属性Buff", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Buff/更改属性Buff", typeof(SkillGraph))]
public class NP_ChangePropertyNode : NP_BuffNodeBase
{
    [LabelText("下面是需要配置的Buff数据")] public NP_BuffNodeDataBase NpBuffNodeDataBase = new NP_BuffNodeDataBase();
    public override string name => "更改属性Buff";

    public override NP_BuffNodeDataBase GetBuffNodeData()
    {
        return NpBuffNodeDataBase;
    }
}