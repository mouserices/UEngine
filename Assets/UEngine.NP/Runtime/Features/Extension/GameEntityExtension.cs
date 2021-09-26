using System.Collections.Generic;
using UEngine.NP.FsmState;

public static class GameEntityExtension
{
    public static bool CheckState(this GameEntity gameEntity, StateType stateType)
    {
        if (!gameEntity.hasState)
        {
            return false;
        }

        if (gameEntity.state.FsmStateBases.First == null)
        {
            return false;
        }

        return gameEntity.state.FsmStateBases.First.Value.StateType == stateType;
    }

    #region Numeric

    /// <summary>
    /// 添加属性修改器
    /// </summary>
    /// <param name="gameEntity"></param>
    /// <param name="t"></param>
    /// <param name="modifier"></param>
    public static void AddNumericModifier(this GameEntity gameEntity, NumericType t, BaseModifier modifier)
    {
        var modifierDic = gameEntity.numericModifier.Modifiers;
        if (modifierDic.TryGetValue(t, out var modifiers))
        {
            modifiers.Add(modifier);
        }
        else
        {
            modifierDic.Add(t, new List<BaseModifier>() {modifier});
        }
    }

    /// <summary>
    /// 计算最终的属性值，并更新到人物NumericComponent组件上
    /// </summary>
    /// <param name="gameEntity"></param>
    /// <param name="t"></param>
    public static void CalculateNumeric(this GameEntity gameEntity, NumericType t)
    {
        var modifierDic = gameEntity.numericModifier.Modifiers;
        if (modifierDic.TryGetValue(t, out var modifiers))
        {
            float constantValue = 0;
            float percentageValue = 0;
            foreach (var modify in modifiers)
            {
                if (modify.ModifierType == ModifierType.Constant)
                {
                    constantValue += modify.GetModifierValue();
                }
                else
                {
                    percentageValue += modify.GetModifierValue();
                }
            }

            var finialValue = constantValue * (1 + percentageValue);

            if (gameEntity.numeric.Numerics.ContainsKey(t))
            {
                gameEntity.numeric.Numerics[t] = finialValue;
            }
            else
            {
                gameEntity.numeric.Numerics.Add(t, finialValue);
            }
        }
    }

    /// <summary>
    /// 获取指定类型的数值
    /// </summary>
    /// <param name="t"></param>
    public static float GetNumeric(this GameEntity gameEntity,NumericType t)
    {
        float value = 0;
        gameEntity.numeric.Numerics.TryGetValue(t, out value);
        return value;
    }

    /// <summary>
    /// 改变指定属性的数值
    /// </summary>
    /// <param name="gameEntity"></param>
    /// <param name="t"></param>
    /// <param name="changeValue"></param>
    public static void ChangeNumeric(this GameEntity gameEntity,NumericType t,float changeValue)
    {
        if (gameEntity.numeric.Numerics.ContainsKey(t))
        {
            gameEntity.numeric.Numerics[t] = gameEntity.numeric.Numerics[t] + changeValue;
        }
    }

    #endregion
}