public class ConstantModifier : BaseModifier
{
    /// <summary>
    /// 修改的值
    /// </summary>
    public float ChangeValue;
    
    public override ModifierType ModifierType => ModifierType.Constant;

    public override float GetModifierValue()
    {
        return ChangeValue;
    }
}