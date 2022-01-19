using System;
using Entitas;
using UEngine.Net;

public class PingSystem : IInitializeSystem, IExecuteSystem
{
    private float _timeInterval = 2f;
    private float _elapsedTime;

    private INetworkService _networkService;
    private ITimeService _timeService;
    public void Initialize()
    {
        var pingEntity = Contexts.sharedInstance.game.CreateEntity();
        pingEntity.AddPing(0);
        _networkService = MetaContext.Get<INetworkService>();
        _timeService = MetaContext.Get<ITimeService>();
    }
    
    public async void Execute()
    {
        if (!MetaContext.Get<INetworkService>().IsConnected())
        {
            return;
        }

        _elapsedTime += _timeService.deltaTime();
        if (_elapsedTime >= _timeInterval)
        {
            _elapsedTime = 0;
            var s2c_Ping_1005 = (S2C_Ping_1005)await _networkService.SendRequest(new C2S_Ping_1004()
                { SendTimestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() });

            var now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            long halfRTT = (now - s2c_Ping_1005.SendTimestamp) / 2;
            
            Contexts.sharedInstance.game.ping.halfRTT = halfRTT;
            
            if (Contexts.sharedInstance.game.hasMainPlayerData)
            {
                var roomID = Contexts.sharedInstance.game.mainPlayerData.RoomID;
                if (roomID > 0)
                {
                    var roomEntity = Contexts.sharedInstance.room.GetEntityWithRoom(roomID);
                    var targetTicksPerFrame = new TimeSpan(TimeSpan.TicksPerSecond / 30);
                    var timeSpan = new TimeSpan(halfRTT);
                    var serverTickCount = (s2c_Ping_1005.ServerTotalTicks + timeSpan.Ticks) / targetTicksPerFrame.Ticks;
                    roomEntity.tick.ServerTickCount = (int)serverTickCount;
                }
            }
        }
    }
}