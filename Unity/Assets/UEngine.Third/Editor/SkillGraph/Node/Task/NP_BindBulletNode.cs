using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;

[NodeMenuItem("NPBehave行为树/Task/BindBullet", typeof (NPBehaveGraph))]
[NodeMenuItem("NPBehave行为树/Task/BindBullet", typeof (SkillGraph))]
public class NP_BindBulletNode : NP_TaskNodeBase
{
    public override string name => "发射子弹";

    public NP_ActionNodeData NpActionNodeData = new NP_ActionNodeData(){NodeType = NodeType.Task, NpClassForStoreAction = new NP_BindBulletAction()};

    public override NP_NodeDataBase NP_GetNodeData()
    {
        return NpActionNodeData;
    }
}