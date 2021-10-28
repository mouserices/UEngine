using NPBehave;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

public class NP_ConditionNodeData : NP_NodeDataBase
{
    private Condition _condition;
    public NP_BaseAction NpClassForStoreAction = new NP_IsConditionMetOfInputKey();

    public override Node NP_GetNode()
    {
        return _condition;
    }

    public override Decorator CreateDecoratorNode(Node node, Skill npRuntimeTree)
    {
        NpClassForStoreAction.Skill = npRuntimeTree;
        _condition = new Condition(NpClassForStoreAction.GetFunc1ToBeDone(), Stops.IMMEDIATE_RESTART, node);
        return _condition;
    }
}