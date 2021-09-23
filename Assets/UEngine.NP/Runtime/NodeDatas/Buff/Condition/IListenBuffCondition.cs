using System;
using Sirenix.OdinInspector;

public interface IListenBuffCondition
{
   Func<bool> GetCondition(long sourceUnitID,int skillID);
}