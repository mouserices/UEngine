using GraphProcessor;

namespace UEngine.NP.Editor
{
    [NodeMenuItem("NPBehave行为树/Task/WaitUntilStopped", typeof (NPBehaveGraph))]
    public class NP_WaitUntilStopped:NP_TaskNodeBase
    {
        public override string name => "停止轮询";

        public NP_WaitUntilStoppedData NodeData = new NP_WaitUntilStoppedData(){NodeType = NodeType.Task};
        public override NP_NodeDataBase NP_GetNodeData()
        {
            return NodeData;
        }
    }
}