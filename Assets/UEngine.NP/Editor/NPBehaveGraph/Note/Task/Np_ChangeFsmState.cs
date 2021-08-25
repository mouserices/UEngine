using GraphProcessor;

namespace UEngine.NP.Editor
{
    [NodeMenuItem("NPBehave行为树/Task/ChangeFsmState", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/ChangeFsmState", typeof (SkillGraph))]
    public class Np_ChangeFsmState: NP_TaskNodeBase
    {
        public override string name =>"切换状态";

        public NP_ActionNodeData NP_PlayAnimNodeData = new NP_ActionNodeData(){NodeType = NodeType.Task,NpClassForStoreAction = new Np_ChangeFsmStateAction()};
        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NP_PlayAnimNodeData;
        }
    } 
}