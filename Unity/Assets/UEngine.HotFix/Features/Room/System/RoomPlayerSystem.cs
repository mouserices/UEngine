using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class RoomPlayerSystem : ReactiveSystem<RoomEntity>
{
    private RoomContext _roomContext;
    public RoomPlayerSystem() : base(Contexts.sharedInstance.room)
    {
        _roomContext = Contexts.sharedInstance.room;
    }

    protected override ICollector<RoomEntity> GetTrigger(IContext<RoomEntity> context)
    {
        return context.CreateCollector(RoomMatcher.AnyOf(RoomMatcher.AddPlayerMessage,
            RoomMatcher.RemovePlayerMessage));
    }

    protected override bool Filter(RoomEntity entity)
    {
        return entity.hasAddPlayerMessage || entity.hasRemovePlayerMessage;
    }

    protected override void Execute(List<RoomEntity> entities)
    {
        foreach (var roomEntity in entities)
        {
            if (roomEntity.hasAddPlayerMessage)
            {
                this.AddPlayer(roomEntity.addPlayerMessage);
            }else if (roomEntity.hasRemovePlayerMessage)
            {
                this.RemovePlayer(roomEntity.removePlayerMessage);
            }
        }
    }

    private void RemovePlayer(RemovePlayerMessageComponent removePlayerMessage)
    {
        var unitID = removePlayerMessage.UnitID;
        var roomID = removePlayerMessage.RoomID;
        var roomEntity = _roomContext.GetRoom(roomID);
        if (roomEntity == null)
        {
            return;
        }

        var removePlayer = roomEntity.room.RemovePlayer(unitID);
        if (removePlayer)
        {
            MetaContext.Get<ILogService>().Log($"Remove Player Succeed from room, unitID: {unitID} roomID: {roomID}");
        }

        //no players,destroy room
        if (roomEntity.room.GetPlayerCount() <= 0)
        {
            roomEntity.Destroy();
            MetaContext.Get<ILogService>().Log($"Room has no player ,begin destroy,roomID: {roomID}");
        }
    }

    private void AddPlayer(AddPlayerMessageComponent addPlayerMessage)
    {
        var roomID = addPlayerMessage.RoomID;
        var connectionId = addPlayerMessage.ConnectionId;
        
        var roomEntity = _roomContext.GetOrCreateRoom(roomID);
        
        //add player to room
        var unitId = addPlayerMessage.UnitId;
        var assets = addPlayerMessage.Asset;
        Vector3 pos = addPlayerMessage.Pos;
        Vector3 rot = addPlayerMessage.Rot;
        var skills = addPlayerMessage.Skills;
        var campType = addPlayerMessage.CampType;
        var playerID = addPlayerMessage.PlayerID;
        roomEntity.room.AddPlayer(unitId,playerID,connectionId,assets,pos,rot,skills,campType);

#if SERVER
        Contexts.sharedInstance.network.session.SetSessionRoomID(connectionId, roomEntity.room.RoomID);
        
        //check max playerCount
        if (roomEntity.room.GetPlayerCount() >= GameDefine.MAX_ROOM_PLAY_COUNT)
        {
            //add tickComponent and begin tick
            roomEntity.AddTick(0,new TimerTick());
            // notify clients enter room
            roomEntity.room.SendMesageToAllPlayers(new S2C_AllPlayerEnterRoom_1008()
            {
                RoomID = roomID,
                Players = roomEntity.room.GetUnitDatas()
            });
        }
#endif
    }
}