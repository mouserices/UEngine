using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
using UEngine.NP;

public enum TriggerConditon
{
    [LabelText("未选择条件")]
    NONE = 0,
    [LabelText("造成伤害时")]
    MAKE_DEMAGE =1 ,
    [LabelText("受到伤害时")]
    GET_DEMAGE =2 ,
}

[Title("Buff节点数据块",TitleAlignment = TitleAlignments.Centered)]
[BsonIgnoreExtraElements]
public class NP_ListenBuffData : NP_BuffNodeDataBase
{
    [BoxGroup("Base Data")][OnValueChanged("OnConditionSelected")]
    public TriggerConditon Condition = TriggerConditon.NONE;

    [BoxGroup("Condition Data")][LabelText("条件参数")]public IListenBuffCondition ConditionAction;

    [BoxGroup("Condition Data")]public List<VTD_Id> BuffEffects = new List<VTD_Id>();
#if UNITY_EDITOR
    private void OnConditionSelected()
    {
        ConditionAction = ConditionFactory.GetConditon(Condition);
    }
#endif

    public override void Excute()
    {
        //base.Excute();
        var buffEntity = Contexts.sharedInstance.buff.CreateEntity();
        buffEntity.AddBuff(this.VtdId.Value,this.UnitID,this.SkillID,this.BehavetreeName);
        buffEntity.AddListenBuff(ConditionAction.GetCondition(this.UnitID,this.SkillID),BuffEffects);
    }
}