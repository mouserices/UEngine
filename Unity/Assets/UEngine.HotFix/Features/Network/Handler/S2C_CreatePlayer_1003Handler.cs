using System.Collections;
using System.Collections.Generic;
using UEngine.Net;
using UEngine.NP.Features.FsmState;
using UEngine.NP.FsmState;
using UnityEngine;

[MessageHandler]
public class S2C_CreatePlayer_1003Handler : MessageHandler<S2C_CreatePlayer_1003>
{
    public override void OnMessageHandle(int connectionId, S2C_CreatePlayer_1003 request)
    {
        
        // MetaContext.Get<IUnitService>()
        //     .CreateUnit(request.Unit.ID, request.Unit.PlayerID, request.Unit.Asset, new Vector3(request.Unit.PosX,request.Unit.PosY,request.Unit.PosZ),
        //         new Vector3(request.Unit.RotX,request.Unit.RotY,request.Unit.RotZ), new IdleStateParam() { AnimClipName = "Idle", StateType = StateType.IDLE },
        //         request.Unit.Skills, (CampType)request.Unit.CampType);
    }
}