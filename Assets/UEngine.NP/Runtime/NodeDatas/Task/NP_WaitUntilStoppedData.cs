//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月26日 18:08:42
//------------------------------------------------------------

using NPBehave;
using Sirenix.OdinInspector;
using UEngine.NP.Unit;

namespace UEngine.NP
{
    [BoxGroup("等待到停止节点数据")]
    [HideLabel]
    public class NP_WaitUntilStoppedData: NP_NodeDataBase
    {
        [LabelText("当Stop时,成功还是失败,默认失败")]
        public bool sucessWhenStopped = false;
        
        [HideInEditorMode]
        private WaitUntilStopped m_WaitUntilStopped;

        public override Node NP_GetNode()
        {
            return this.m_WaitUntilStopped;
        }

        public override Task CreateTask(NP_RuntimeTree npRuntimeTree,long unitID)
        {
            this.m_WaitUntilStopped = new WaitUntilStopped(sucessWhenStopped);
            return this.m_WaitUntilStopped;
        }
    }
}