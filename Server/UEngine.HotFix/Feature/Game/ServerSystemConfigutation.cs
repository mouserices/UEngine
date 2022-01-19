using System.Net.Sockets;
using UEngine.Net;
using UEngine.NP;

public class ServerSystemConfigutation : Feature, ISystemConfiguration
{
    public void InitializeSystems(Contexts contexts)
    {
        Add(new AutoRegisterServiceSystem());
        Add(new NetworkSystems());
        Add(new BehaveTreeFactorySystem(contexts));
        Add(new ClockSystem());

        Add(new LoginSystems());
        Add(new RoomSystems());
        Add(new FrameSyncSystems());
    }
}