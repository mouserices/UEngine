//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:12:40
//------------------------------------------------------------

using System.Collections.Generic;
using NPBehave;
using Sirenix.OdinInspector;

namespace UEngine.NP
{
    public class NP_BlackboardQueryNodeData: NP_NodeDataBase
    {
        [HideInEditorMode]
        private BlackboardQuery m_BlackboardQueryNode;

        public override Node NP_GetNode()
        {
            return this.m_BlackboardQueryNode;
        }
        
        public override Decorator CreateDecoratorNode(Node node,Skill npRuntimeTree)
        {
            return base.CreateDecoratorNode(node,npRuntimeTree);
        }

    }
}