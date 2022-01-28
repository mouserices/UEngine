using System.Collections.Generic;
using Entitas;
using UEngine.Net;
using UnityEngine;

public class GameStartSystem : ReactiveSystem<NetworkEntity>
{
    private GameContext _gameContext;
    public GameStartSystem() : base(Contexts.sharedInstance.network)
    {
        _gameContext = Contexts.sharedInstance.game;
    }

    protected override ICollector<NetworkEntity> GetTrigger(IContext<NetworkEntity> context)
    {
        return context.CreateCollector(NetworkMatcher.NetworkConnected);
    }

    protected override bool Filter(NetworkEntity entity)
    {
        return entity.hasNetworkConnected;
    }

    protected async override void Execute(List<NetworkEntity> entities)
    {
        if (entities.Count <= 0)
        {
            return;
        }

        MetaContext.Get<ILogService>().Log("Socket Connected");
        var response = (S2C_Login)await MetaContext.Get<INetworkService>()
            .SendRequest(new C2S_Login() { UserName = "zs", Password = "1234" });

        if (!_gameContext.hasMainPlayerData)
        {
            _gameContext.SetMainPlayerData(response.PlayerID,0);
        }
        
        var s2CEnterRoom1007 = (S2C_EnterRoom_1007)await MetaContext.Get<INetworkService>()
            .SendRequest(new C2S_EnterRoom_1006() { RoomID = 1});
        if (s2CEnterRoom1007.Succeed)
        {
            MetaContext.Get<ILogService>().Log("EnterRoom Succeed!");
        }
        else
        {
            MetaContext.Get<ILogService>().Log("EnterRoom Failed!");

        }
    }
}