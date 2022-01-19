using System.Collections.Generic;
using Entitas;
using UEngine.Net;

/// <summary>
/// 处理session的缓存
/// 1.创建
/// 2.销毁
/// </summary>
public class SessionSystem : ReactiveSystem<NetworkEntity>
{
    private RoomContext _roomContext;
    private UnitContext _unitContext;
    public SessionSystem() : base(Contexts.sharedInstance.network)
    {
        _roomContext = Contexts.sharedInstance.room;
        _unitContext = Contexts.sharedInstance.unit;
    }

    protected override ICollector<NetworkEntity> GetTrigger(IContext<NetworkEntity> context)
    {
        return context.CreateCollector(NetworkMatcher.AnyOf(NetworkMatcher.NetworkConnected,
            NetworkMatcher.NetworkDisconnected));
    }

    protected override bool Filter(NetworkEntity entity)
    {
        return entity.hasNetworkConnected || entity.hasNetworkDisconnected;
    }

    protected override void Execute(List<NetworkEntity> entities)
    {
        foreach (var networkEntity in entities)
        {
            if (networkEntity.hasNetworkConnected)
            {
                DoCreateSession(networkEntity.networkConnected);
            }

            if (networkEntity.hasNetworkDisconnected)
            {
                DoClearSession(networkEntity.networkDisconnected);
            }
        }
    }

    private void DoClearSession(NetworkDisconnectedComponent networkDisconnectedComponent)
    {
        var connectedID = networkDisconnectedComponent.ConnectedID;
        var networkContext = Contexts.sharedInstance.network;
        if (networkContext.session.ConnectedIDToSessions.TryGetValue(connectedID,out var session))
        {
            networkContext.session.ConnectedIDToSessions.Remove(connectedID);

            var unitEntity = _unitContext.GetEntityWithConnectionID(connectedID);
            _roomContext.CreateEntity().AddRemovePlayerMessage(unitEntity.unit.ID,session.RoomID);
            MetaContext.Get<ILogService>().Log($"ClearSession Succeed, connectedID:{connectedID}");
        }
        else
        {
            MetaContext.Get<ILogService>().LogError($"ClearSession Error, connectedID:{connectedID} not found");
        }
    }

    private void DoCreateSession(NetworkConnectedComponent networkConnectedComponent)
    {
        var networkContext = Contexts.sharedInstance.network;
        if (!networkContext.hasSession)
        {
            networkContext.SetSession(new Dictionary<int, Session>());
        }

        var connectedID = networkConnectedComponent.ConnectedID;
        if (!networkContext.session.ConnectedIDToSessions.ContainsKey(connectedID))
        {
            networkContext.session.ConnectedIDToSessions.Add(connectedID,new Session(){ConnectedID = connectedID});
            MetaContext.Get<ILogService>().Log($"CreateSession Succeed, connectedID:{connectedID}");

        }
        else
        {
            MetaContext.Get<ILogService>().LogError($"Create Session Error, has contain same connectedID: {connectedID}");
        }
    }
}