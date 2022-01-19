public class FixedModifier : BaseModifier
{
    /// <summary>
    /// 固定的值
    /// </summary>
    public float FixedValue;
    
    public override ModifierType ModifierType => ModifierType.Constant;

    public override float GetModifierValue()
    {
        return FixedValue;
    }
}