using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Task/ClearInputKey", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/ClearInputKey", typeof(SkillGraph))]
public class NP_ClearInputNode : NP_TaskNodeBase
{
    public override string name => "清除键盘输入";

    public NP_ActionNodeData NpActionNodeData = new NP_ActionNodeData(){NodeType = NodeType.Task, NpClassForStoreAction = new NP_ClearInputAction()};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpActionNodeData;
    }
}