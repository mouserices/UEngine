using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Task/ClearComboKey", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/ClearComboKey", typeof (SkillGraph))]
public class NP_ClearComboNode : NP_TaskNodeBase
{
    public override string name => "延迟清除连击次数";

    public NP_ActionNodeData NpActionNodeData = new NP_ActionNodeData(){NodeType = NodeType.Task, NpClassForStoreAction = new NP_ClearComboAction()};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpActionNodeData;
    }
}