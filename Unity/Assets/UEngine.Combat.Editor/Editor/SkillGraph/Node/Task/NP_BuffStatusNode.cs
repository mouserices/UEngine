
using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;
[NodeMenuItem("NPBehave行为树/Task/BuffStatus", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/BuffStatus", typeof (SkillGraph))]
public class NP_BuffStatusNode : NP_TaskNodeBase
{
    public override string name =>"添加Buff状态";
    public NP_ActionNodeData NpActionNodeData = new NP_ActionNodeData(){NpClassForStoreAction = new NP_BuffStatusAction()};
    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpActionNodeData;
    }
}