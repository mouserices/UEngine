using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;
using UnityEngine;

[NodeMenuItem("NPBehave行为树/Task/绑定Buff", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/绑定Buff", typeof (SkillGraph))]
public class NP_BindBuffNode : NP_TaskNodeBase
{
    public override string name => "绑定Buff";
    public override Color color => Color.green;

    public NP_ActionNodeData NpActionNodeData = new NP_ActionNodeData(){NodeType = NodeType.Task, NpClassForStoreAction = new NP_BindBuffAction()};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpActionNodeData;
    }
}