using System.Threading.Tasks;
using UEngine.Net;

public interface INetworkService : IService
{
    void StartClient(string ip, int port);
    void StartServer(int port);
    NetworkRequest SendRequest<T>(T message) where T : IMessage;
    NetworkRequest SendRequest<T>(int connectedID, T message) where T : IMessage;
    void SendResponse<T>(int connectedID, T message) where T : IResponse;

    void SendMessage<T>(int connectedID, T message) where T : ISimpleMessage;

    bool IsConnected();
}