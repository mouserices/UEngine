using System;
using System.Collections.Generic;
using UnityEngine;

namespace UEngine.Net.Handler
{
    [MessageHandler]
    public class C2S_EnterRoom_1006Handler : MessageHandler<C2S_EnterRoom_1006, S2C_EnterRoom_1007>
    {
        public override void OnMessageHandle(int connectionId, C2S_EnterRoom_1006 request,
            Action<S2C_EnterRoom_1007> response)
        {
            var roomContext = Contexts.sharedInstance.room;
            var roomEntity = roomContext.GetOrCreateRoom(request.RoomID);

            //Check Max PlayerCount
            if (roomEntity.room.IsMaxPlayerCount())
            {
                response(new S2C_EnterRoom_1007() { Succeed = false });
                return;
            }
            response(new S2C_EnterRoom_1007() { Succeed = true });

            var unitId = UnitIdGenerater.GetID();
            var assets = "eliteKnight";
            Vector3 pos = new Vector3(0, 0, -15f);
            Vector3 rot = new Vector3(0, 0, 0);
            var skills = new List<int>() { 1001 };
            var campType = unitId % 2 == 0 ? CampType.Own : CampType.Enemy;
            var playerID = Contexts.sharedInstance.network.session.GetPlayerIDByConnectionID(connectionId);
            
            roomContext.CreateEntity().AddAddPlayerMessage(request.RoomID,connectionId,unitId,playerID,assets,pos,rot,skills,campType);
            
        }
    }
}