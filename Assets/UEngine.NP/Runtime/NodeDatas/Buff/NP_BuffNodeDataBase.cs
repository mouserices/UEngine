using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEngine;

[BsonDeserializerRegister]
public class NP_BuffNodeDataBase
{
    [HideInInspector]
    public long UnitID;
    [HideInInspector]
    public int SkillID;
    [HideInInspector]
    public string BehavetreeName;
    [BoxGroup("Base Data")]
    [LabelText("BuffID")]
    public VTD_Id VtdId;
    public virtual void Excute()
    {
        Debug.Log($"change property succeed! {Time.frameCount}");
    }
}