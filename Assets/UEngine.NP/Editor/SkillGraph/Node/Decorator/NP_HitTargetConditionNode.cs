using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Decorator/HitTargetConditon", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Decorator/HitTargetConditon", typeof (SkillGraph))]
public class NP_HitTargetConditionNode : NP_DecoratorNodeBase
{
    public override string name => "监听子弹是否命中";

    public NP_ConditionNodeData NpConditionNodeData =
        new NP_ConditionNodeData { NpClassForStoreAction = new NP_HitTargetConditionAction(), NodeType = NodeType.Decorator};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpConditionNodeData;
    }
}