using System;

public class MakeDamageCondition : IListenBuffCondition
{
    public Func<bool> GetCondition(long sourceUnitID,int skillID)
    {
        Func<bool> func = () =>
        {
            var entity = Contexts.sharedInstance.unit.GetEntityWithUnit(sourceUnitID);
            return entity.isAP_AttackSucceed;
        };

        return func;
    }
}