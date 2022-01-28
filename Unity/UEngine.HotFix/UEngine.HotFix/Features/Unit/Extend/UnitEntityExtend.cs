using System.Collections.Generic;
using UEngine.Net;

public static class UnitEntityExtend
{
    public static UnitData GetUnitData(this UnitEntity unitEntity)
    {
        UnitData unitData = new UnitData();
        unitData.ID = (int)unitEntity.unit.ID;
        unitData.Skills = new List<int>() { 1001 };
        unitData.CampType = (int)unitEntity.camp.Value;
        unitData.PosX = unitEntity.position.value.x;
        unitData.PosY = unitEntity.position.value.y;
        unitData.PosZ = unitEntity.position.value.z;

        unitData.RotX = unitEntity.rotation.Value.x;
        unitData.RotY = unitEntity.rotation.Value.y;
        unitData.RotZ = unitEntity.rotation.Value.z;
        unitData.PlayerID = unitEntity.playerID.ID;
        return unitData;
    }

}