using System;
using System.Collections.Generic;
using System.IO;
using Entitas;

namespace UEngine.Net
{
    public class NetworkStartSystem : ReactiveSystem<NetworkEntity>
    {
        private MemoryStream receivedStream;
        private NetMessageEntity receivedEntity;

        public NetworkStartSystem() : base(Contexts.sharedInstance.network)
        {
            receivedStream = new MemoryStream();
            receivedEntity = Contexts.sharedInstance.netMessage.CreateEntity();
        }

        protected override ICollector<NetworkEntity> GetTrigger(IContext<NetworkEntity> context)
        {
            return context.CreateCollector(NetworkMatcher.AnyOf(NetworkMatcher.ClientConnecting,
                NetworkMatcher.ServerListening));
        }

        protected override bool Filter(NetworkEntity entity)
        {
            return entity.hasClientConnecting || entity.hasServerListening;
        }

        protected override void Execute(List<NetworkEntity> entities)
        {
            foreach (NetworkEntity networkEntity in entities)
            {
                if (networkEntity.hasClientConnecting)
                {
                    DoStartClient(networkEntity.clientConnecting.IP, networkEntity.clientConnecting.Port);
                }
                else if (networkEntity.hasServerListening)
                {
                    DoStartServer(networkEntity.serverListening.Port);
                }
            }
        }

        private void DoStartServer(int networkServerPort)
        {
            Telepathy.Server server = new Telepathy.Server(16 * 1024);
            server.OnConnected = (connectionId) => Console.Write(connectionId + " Connected");
            server.OnData = this.OnServerDataReceived;
            server.OnDisconnected = (connectionId) => Console.Write(connectionId + " Disconnected");

            server.Start(networkServerPort);
            Contexts.sharedInstance.network.serverListeningEntity.AddNetworkServer(server);

            Console.Write($"server start,port:{networkServerPort}");
        }

        private void OnServerDataReceived(int connectionId, ArraySegment<byte> data)
        {
            var netMessageEntity = Contexts.sharedInstance.netMessage.CreateEntity();
            netMessageEntity.AddMessageDispacher(data.Array);
        }

        private void DoStartClient(string ip, int port)
        {
            Telepathy.Client client = new Telepathy.Client(16 * 1024);
            client.OnConnected = () => Console.Write("connected succeed!");
            client.OnData = this.OnDataReceived;
            client.OnDisconnected = () => Console.Write("Client Disconnected!");
            client.Connect(ip, port);
            Contexts.sharedInstance.network.clientConnectingEntity.AddNetworkClient(client);

            Console.Write($"client connecting,ip:{ip} port:{port}");
        }

        private void OnDataReceived(ArraySegment<byte> message)
        {
            /*var netMessageEntity = Contexts.sharedInstance.netMessage.CreateEntity();
            netMessageEntity.AddMessageStream(OpType.Parsing, message.Array);*/
        }
    }
}