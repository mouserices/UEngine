using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Task/DamageBuff", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/DamageBuff", typeof(SkillGraph))]
public class NP_DamageBuffNode : NP_TaskNodeBase
{
    public override string name => "造成伤害";

    public NP_ActionNodeData NpActionNodeData = new NP_ActionNodeData()
        {NodeType = NodeType.Task, NpClassForStoreAction = new NP_DamageBuffAction()};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpActionNodeData;
    }
}