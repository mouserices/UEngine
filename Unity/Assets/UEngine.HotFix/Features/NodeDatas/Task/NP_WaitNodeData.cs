//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:13:45
//------------------------------------------------------------

using NPBehave;
using Sirenix.OdinInspector;

namespace UEngine.NP
{
    [BoxGroup("等待结点数据")]
    [HideLabel]
    public class NP_WaitNodeData : NP_NodeDataBase
    {
        [HideInEditorMode] private Wait m_WaitNode;

        //public NP_BlackBoardRelationData NPBalckBoardRelationData = new NP_BlackBoardRelationData();
        public float Second;
        public float Speed;

        public override Task CreateTask(Skill npRuntimeTree, long unitID)
        {
            this.m_WaitNode = new Wait(Second / Speed);
            return this.m_WaitNode;
        }

        public override Node NP_GetNode()
        {
            return this.m_WaitNode;
        }
    }
}