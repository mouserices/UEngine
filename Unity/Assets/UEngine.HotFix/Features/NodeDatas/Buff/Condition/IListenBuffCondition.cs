using System;

public interface IListenBuffCondition
{
   Func<bool> GetCondition(long sourceUnitID,int skillID);
}