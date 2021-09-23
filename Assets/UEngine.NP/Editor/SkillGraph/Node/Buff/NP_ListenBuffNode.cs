using GraphProcessor;
using Sirenix.OdinInspector;

[NodeMenuItem("NPBehave行为树/Buff/监听Buff", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Buff/监听Buff", typeof(SkillGraph))]
public class NP_ListenBuffNode : NP_BuffNodeBase
{
    [LabelText("下面是需要配置的Buff数据")] public NP_BuffNodeDataBase NpBuffNodeDataBase = new NP_ListenBuffData();
    public override string name => "监听Buff";

    public override NP_BuffNodeDataBase GetBuffNodeData()
    {
        return NpBuffNodeDataBase;
    }

    public override void AutoAddLinkedBuffs()
    {
        base.AutoAddLinkedBuffs();
        var npListenBuffData = NpBuffNodeDataBase as NP_ListenBuffData;
        npListenBuffData.BuffEffects.Clear();

        var outputNodes = this.GetOutputNodes();
        foreach (BaseNode outputNode in outputNodes)
        {
            if (outputNode is NP_BuffNodeBase connectedBuffNode)
            {
                var vtdId = connectedBuffNode.GetBuffNodeData().VtdId;
                npListenBuffData.BuffEffects.Add(vtdId);
            }
        }
    }
}