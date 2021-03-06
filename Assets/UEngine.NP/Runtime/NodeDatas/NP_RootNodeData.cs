//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:09:18
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace UEngine.NP
{
    public class NP_RootNodeData: NP_NodeDataBase
    {
        [HideInEditorMode]
        public Root m_Root;

        public override Decorator CreateDecoratorNode(Node node,NP_RuntimeTree npRuntimeTree)
        {
            this.m_Root = new Root(node);
            return this.m_Root;
        }

        public override Node NP_GetNode()
        {
            return this.m_Root;
        }
    }
}