
public enum ModifierType
{
    /// <summary>
    /// 常数修改器类型
    /// </summary>
    Constant,

    /// <summary>
    /// 百分比修改器类型
    /// </summary>
    Percentage
}

public abstract class BaseModifier
{
    /// <summary>
    /// 修改器类型
    /// </summary>
    public abstract ModifierType ModifierType { get; }


    /// <summary>
    /// 获取修改值
    /// </summary>
    /// <returns></returns>
    public abstract float GetModifierValue();
}