using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Task/MoveToTarget", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/MoveToTarget", typeof (SkillGraph))]
public class NP_MoveToTargetNode : NP_TaskNodeBase
{
    public override string name =>"移动到目标点";

    public NP_ActionNodeData NP_PlayAnimNodeData = new NP_ActionNodeData(){NodeType = NodeType.Task,NpClassForStoreAction = new NP_MoveToTargetAction()};
    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_PlayAnimNodeData;
    }
}