using GraphProcessor;

namespace UEngine.NP.Editor
{
    [NodeMenuItem("NPBehave行为树/Task/RemoveFsmState", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/RemoveFsmState", typeof (SkillGraph))]
    public class NP_RemoveFsmState : NP_TaskNodeBase
    {
        public override string name =>"移除状态";

        public NP_ActionNodeData NpActionNodeData = new NP_ActionNodeData(){NodeType = NodeType.Task,NpClassForStoreAction = new NP_RemoveFsmStateAction()};
        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NpActionNodeData;
        }
    }
}