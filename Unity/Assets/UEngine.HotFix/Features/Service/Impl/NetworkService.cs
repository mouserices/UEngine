using System.Collections.Generic;
using System.Threading.Tasks;
using UEngine.Net;
using UnityEngine;

[Service]
public class NetworkService : INetworkService
{
    public void StartClient(string ip, int port)
    {
        var networkEntity = Contexts.sharedInstance.network.CreateEntity();
        networkEntity.AddClientConnecting(ip, port);
    }

    public void StartServer(int port)
    {
        var networkEntity = Contexts.sharedInstance.network.CreateEntity();
        networkEntity.AddServerListening(port);
    }

    public NetworkRequest SendRequest<T>(T message) where T : IMessage
    {
        return SendRequest(0, message);
    }

    public NetworkRequest SendRequest<T>(int connectedID,T request) where T : IMessage
    {
        Debug.Log("SendRequest");
        request.RpcID = RpcIdGenerater.GetRpcID();
        var networkRequest = new NetworkRequest(request.RpcID);
        
        var networkContext = Contexts.sharedInstance.network;
        if (!networkContext.hasNetworkRequest)
        {
            networkContext.SetNetworkRequest(new Dictionary<int, NetworkRequest>());
        }

        networkContext.networkRequest.Requests.Add(networkRequest.RpcId, networkRequest);

        if (networkContext.messageRelation.Type2Ops.TryGetValue(request.GetType(),out int opCode))
        {
            NetMessageEntity netMessageEntity = Contexts.sharedInstance.netMessage.CreateEntity();
            netMessageEntity.AddMessageSend(connectedID,opCode,request);
        }
        return networkRequest;
    }

    public void SendResponse<T>(int connectedID, T message) where T : IResponse
    {
        var networkContext = Contexts.sharedInstance.network;
        if (networkContext.messageRelation.Type2Ops.TryGetValue(message.GetType(),out int opCode))
        {
            NetMessageEntity netMessageEntity = Contexts.sharedInstance.netMessage.CreateEntity();
            netMessageEntity.AddMessageSend(connectedID,opCode,message);
        }
    }

    public void SendMessage<T>(int connectedID,T request) where T : ISimpleMessage
    {
        var networkContext = Contexts.sharedInstance.network;
        if (networkContext.messageRelation.Type2Ops.TryGetValue(request.GetType(),out int opCode))
        {
            NetMessageEntity netMessageEntity = Contexts.sharedInstance.netMessage.CreateEntity();
            netMessageEntity.AddMessageSend(connectedID,opCode,request);
        }
    }

    public bool IsConnected()
    {
        var networkContext = Contexts.sharedInstance.network;
        if (networkContext.hasNetworkClient && networkContext.networkClient.Client != null && networkContext.networkClient.Client.Connected)
        {
            return true;
        }

        return false;
    }
}