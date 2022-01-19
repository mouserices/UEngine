using Sirenix.OdinInspector;
using UEngine.NP;
#if CLIENT
    using UnityEngine;
#endif

[BsonDeserializerRegister]
public class NP_BuffNodeDataBase
{
#if CLIENT
    [HideInInspector]
#endif
    public long UnitID;
#if CLIENT
    [HideInInspector]
#endif
    public int SkillID;
#if CLIENT
    [HideInInspector]
#endif
    public string BehavetreeName;
    [BoxGroup("Base Data")]
    [LabelText("BuffID")]
    public VTD_Id VtdId;
    public virtual void Excute()
    {
        //Debug.Log($"change property succeed! {Time.frameCount}");
    }
}