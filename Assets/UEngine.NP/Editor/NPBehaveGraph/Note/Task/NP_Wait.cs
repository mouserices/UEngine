using GraphProcessor;

namespace UEngine.NP.Editor
{
    [NodeMenuItem("NPBehave行为树/Task/Wait", typeof (NPBehaveGraph))]
    [NodeMenuItem("NPBehave行为树/Task/Wait", typeof (SkillGraph))]
    public class NP_Wait : NP_TaskNodeBase
    {
        public override string name => "等待某个时间";
        public NP_WaitNodeData NpWaitNodeData = new NP_WaitNodeData(){NodeType = NodeType.Task};
        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NpWaitNodeData;
        }
    }
}