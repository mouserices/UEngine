//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 13:11:41
//------------------------------------------------------------

using GraphProcessor;

namespace UEngine.NP.Editor
{
    public abstract class NP_NodeBase : BaseNode
    {
        public virtual NP_NodeDataBase NP_GetNodeData()
        {
            return null;
        }
    }
}