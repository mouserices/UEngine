using GraphProcessor;

namespace UEngine.NP.Editor
{
    [NodeMenuItem("NPBehave行为树/Task/PlayAnim", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/PlayAnim", typeof (SkillGraph))]
    public class NP_PlayAnimNode: NP_TaskNodeBase
    {
        public override string name =>"播放动画";

        public NP_ActionNodeData NP_PlayAnimNodeData = new NP_ActionNodeData(){NodeType = NodeType.Task,NpClassForStoreAction = new NP_PlayAnimAction()};
        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_PlayAnimNodeData;
        }
    } 
}