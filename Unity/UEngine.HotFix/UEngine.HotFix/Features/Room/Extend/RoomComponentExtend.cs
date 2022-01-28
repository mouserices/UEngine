using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UEngine.Net;
using UEngine.NP.Features.FsmState;
using UEngine.NP.FsmState;
using UnityEngine;

public static class RoomComponentExtend
{
    public static bool AddPlayer(this RoomComponent roomComponent,int unitId,long playerID,int connectionID,string asset,
        Vector3 pos,Vector3 rot,List<int> skills,CampType campType)
    {
        if (roomComponent.Players.TryGetValue(unitId, out var entity))
        {
            return false;
        }
        
        var unitEntity = MetaContext.Get<IUnitService>()
            .CreateUnit(unitId, playerID,connectionID, asset, pos,
                rot, new IdleStateParam() { AnimClipName = "Idle", StateType = StateType.IDLE },
                skills, campType);
        roomComponent.Players.Add(unitId, unitEntity);
        return true;
    }

    public static bool RemovePlayer(this RoomComponent roomComponent, long unitID)
    {
        var unitEntity = Contexts.sharedInstance.unit.GetEntityWithUnit(unitID);
        if (unitEntity == null)
        {
            return false;
        }

        if (!unitEntity.hasUnit)
        {
            return false;
        }

        if (!roomComponent.Players.ContainsKey(unitID))
        {
            return false;
        }

        roomComponent.Players.Remove(unitID);
        unitEntity.Destroy();
        return true;
    }

    public static int GetPlayerCount(this RoomComponent roomComponent)
    {
        return roomComponent.Players.Count;
    }

#if SERVER
     public static bool IsMaxPlayerCount(this RoomComponent roomComponent)
    {
        return roomComponent.GetPlayerCount() >= GameDefine.MAX_ROOM_PLAY_COUNT;
    }

    public static void SendMesageToAllPlayers(this RoomComponent roomComponent, ISimpleMessage message)
    {
        foreach (var unitEntity in roomComponent.Players.Values)
        {
            MetaContext.Get<INetworkService>().SendMessage((int)unitEntity.unit.ID, message);
        }
    }
#endif

    public static List<UnitData> GetUnitDatas(this RoomComponent roomComponent)
    {
        List<UnitData> playerDatas = new List<UnitData>();
        foreach (var unitEntity in roomComponent.Players.Values)
        {
            playerDatas.Add(unitEntity.GetUnitData());
        }

        return playerDatas;
    }
}