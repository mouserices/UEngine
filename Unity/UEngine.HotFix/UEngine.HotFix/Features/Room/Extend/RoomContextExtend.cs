using System.Collections.Generic;

public static class RoomContextExtend
{
    public static RoomEntity GetOrCreateRoom(this RoomContext roomContext,int roomID)
    {
        var roomEntity = roomContext.GetEntityWithRoom(roomID);
        if (roomEntity == null)
        {
            roomEntity = roomContext.CreateEntity();
            roomEntity.AddRoom(roomID,new Dictionary<long, UnitEntity>());
        }
        return roomEntity;
    }

    public static RoomEntity GetRoom(this RoomContext roomContext,int roomID)
    {
        var roomEntity = roomContext.GetEntityWithRoom(roomID);
        return roomEntity;
    }
}