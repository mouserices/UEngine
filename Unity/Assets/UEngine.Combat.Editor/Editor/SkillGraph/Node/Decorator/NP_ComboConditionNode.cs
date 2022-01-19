using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Decorator/ComboConditon", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Decorator/ComboConditon", typeof (SkillGraph))]
public class NP_ComboConditionNode : NP_DecoratorNodeBase
{
    public override string name => "监听连击次数";

    public NP_ConditionNodeData NpConditionNodeData =
        new NP_ConditionNodeData { NodeType = NodeType.Decorator,NpClassForStoreAction = new NP_ComboConditionAction()};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpConditionNodeData;
    }
}