using System.Collections.Generic;

public static class ConditionFactory
{
    private static Dictionary<TriggerConditon, IListenBuffCondition> _conditions =
        new Dictionary<TriggerConditon, IListenBuffCondition>();

    static ConditionFactory()
    {
        RegisterCondition(TriggerConditon.MAKE_DEMAGE,new MakeDamageCondition());
    }

    private static void RegisterCondition(TriggerConditon tc, IListenBuffCondition condition)
    {
        if (!_conditions.ContainsKey(tc))
        {
            _conditions.Add(tc, condition);
        }
    }

    public static IListenBuffCondition GetConditon(TriggerConditon tc)
    {
        return _conditions[tc];
    }
}