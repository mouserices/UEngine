using System;
using UnityEngine;

public class PlayerInputTickSystem : ITickSystem
{
    public void Tick()
    {
        var roomEntities = Contexts.sharedInstance.room.GetEntities();
        foreach (var roomEntity in roomEntities)
        {
            if (roomEntity.hasTick)
            {
                //MetaContext.Get<ILogService>().Log($"RoomID: {roomEntity.room.RoomID} TickCount: {roomEntity.tick.TickCount}");
            }
        }
    }
}