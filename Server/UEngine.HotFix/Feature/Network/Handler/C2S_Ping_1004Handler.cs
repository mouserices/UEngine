using System;

namespace UEngine.Net.Handler
{
    [MessageHandler]
    public class C2S_Ping_1004Handler : MessageHandler<C2S_Ping_1004,S2C_Ping_1005>
    {
        public override void OnMessageHandle(int connectionId, C2S_Ping_1004 request, Action<S2C_Ping_1005> response)
        {
            long serverTotalTicks = 0;
            var session = Contexts.sharedInstance.network.session.GetSessionByConnectionID(connectionId);
            if (session.RoomID > 0)
            {
                serverTotalTicks = Contexts.sharedInstance.room.GetEntityWithRoom(session.RoomID).tick.TimerTick.TotalTimeWithPause.Ticks;
            }
            response(new S2C_Ping_1005(){SendTimestamp = request.SendTimestamp,ServerTotalTicks = serverTotalTicks});
        }
    }
}