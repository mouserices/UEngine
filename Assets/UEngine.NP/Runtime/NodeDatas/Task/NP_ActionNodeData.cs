//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月21日 7:13:30
//------------------------------------------------------------

using NPBehave;
using Sirenix.OdinInspector;
using UEngine.NP.Unit;

namespace UEngine.NP
{
    [BoxGroup("行为结点数据")]
    [HideLabel]
    public class NP_ActionNodeData : NP_NodeDataBase
    {
        [HideInEditorMode] private Action m_ActionNode;
        
        public NP_ClassForStoreAction NpClassForStoreAction;

        public override Task CreateTask(NP_RuntimeTree npRuntimeTree,BaseUnit baseUnit)
        {
            this.NpClassForStoreAction.BaseUnit = baseUnit;
            this.NpClassForStoreAction.BelongtoRuntimeTree = npRuntimeTree;
            this.m_ActionNode = this.NpClassForStoreAction._CreateNPBehaveAction();
            return this.m_ActionNode;
        }

        public override Node NP_GetNode()
        {
            return this.m_ActionNode;
        }
    }
}