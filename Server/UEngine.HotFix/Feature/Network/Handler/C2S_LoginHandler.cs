using System;
using System.Collections.Generic;
using UEngine.NP.Features.FsmState;
using UEngine.NP.FsmState;
using UnityEngine;

namespace UEngine.Net.Handler
{
    [MessageHandler]
    public class C2S_LoginHandler : MessageHandler<C2S_Login, S2C_Login>
    {
        public override void OnMessageHandle(int connectionId, C2S_Login request, Action<S2C_Login> response)
        {
            MetaContext.Get<ILogService>().Log($"S2C_LoginHandler: {request.UserName} {request.Password}");
            var playerID = PlayerIDGenerator.GetID();
            Contexts.sharedInstance.network.session.SetSessionPlayerID(connectionId,playerID);
            response(new S2C_Login() { UserName = "ww", Password = request.Password ,PlayerID = playerID});

            var loginEntity = Contexts.sharedInstance.game.CreateEntity();
            loginEntity.AddLoginMessage(connectionId,playerID,request.UserName,request.Password);
        }
    }
}