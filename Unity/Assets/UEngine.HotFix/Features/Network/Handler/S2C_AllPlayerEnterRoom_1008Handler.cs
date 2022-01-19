using UEngine.Net;
using UEngine.NP.Features.FsmState;
using UEngine.NP.FsmState;
using UnityEngine;

[MessageHandler]
public class S2C_AllPlayerEnterRoom_1008Handler : MessageHandler<S2C_AllPlayerEnterRoom_1008>
{
    public override void OnMessageHandle(int connectionId, S2C_AllPlayerEnterRoom_1008 request)
    {
        var roomContext = Contexts.sharedInstance.room;
        var gameContext = Contexts.sharedInstance.game;
        var roomEntity = roomContext.GetOrCreateRoom(request.RoomID);
        roomEntity.AddTick(0,new TimerTick(),0);
        gameContext.mainPlayerData.RoomID = request.RoomID;
        
        foreach (UnitData unitData in request.Players)
        {
            roomContext.CreateEntity().AddAddPlayerMessage(request.RoomID,0,unitData.ID,unitData.PlayerID,"eliteKnight",new Vector3(unitData.PosX,unitData.PosY,unitData.PosZ),
                new Vector3(unitData.RotX,unitData.RotY,unitData.RotZ),unitData.Skills,(CampType)unitData.CampType);
        }
    }
}