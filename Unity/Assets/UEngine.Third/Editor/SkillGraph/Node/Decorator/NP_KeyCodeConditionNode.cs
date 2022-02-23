using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Decorator/InputKeyConditon", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Decorator/InputKeyConditon", typeof (SkillGraph))]
public class NP_KeyCodeConditionNode : NP_DecoratorNodeBase
{
    public override string name => "监听键盘输入";

    public NP_ConditionNodeData NpConditionNodeData =
        new NP_ConditionNodeData { NodeType = NodeType.Decorator};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpConditionNodeData;
    }
}