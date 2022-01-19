using System.Collections.Generic;
using UnityEngine;

public interface IUnitService : IService
{
    UnitEntity CreateUnit(int id,long playerID,int connectionId,string asset,Vector3 pos,Vector3 rot,
      StateParam stateParam,List<int> skills,CampType campType);
}