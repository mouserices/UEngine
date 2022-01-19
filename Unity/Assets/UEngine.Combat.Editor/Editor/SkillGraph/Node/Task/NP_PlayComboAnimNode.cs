using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Task/PlayComboAnim", typeof(NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/PlayComboAnim", typeof(SkillGraph))]
public class NP_PlayComboAnimNode : NP_TaskNodeBase
{
    public override string name => "播放连击动画";

    public NP_ActionNodeData NP_PlayAnimNodeData = new NP_ActionNodeData()
        {NodeType = NodeType.Task, NpClassForStoreAction = new NP_PlayComboAnimAction()};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NP_PlayAnimNodeData;
    }
}