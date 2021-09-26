public class PercentageModifier : BaseModifier
{
    /// <summary>
    /// 百分比
    /// </summary>
    public float Percentage;
    
    public override ModifierType ModifierType => ModifierType.Percentage;

    public override float GetModifierValue()
    {
        return Percentage;
    }
}