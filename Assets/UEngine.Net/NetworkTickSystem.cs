using Entitas;

namespace UEngine.Net
{
    public class NetworkTickSystem : IExecuteSystem
    {
        private IGroup<NetworkEntity> tickGroup;

        public NetworkTickSystem()
        {
            tickGroup = Contexts.sharedInstance.network.GetGroup(NetworkMatcher.AnyOf(NetworkMatcher.NetworkClient,
                NetworkMatcher.NetworkServer));
        }

        public void Execute()
        {
            if (tickGroup.count > 0)
            {
                foreach (NetworkEntity networkEntity in tickGroup.GetEntities())
                {
                    if (networkEntity.hasNetworkClient)
                    {
                        networkEntity.networkClient.Client.Tick(100);
                    }
                    else if (networkEntity.hasNetworkServer)
                    {
                        networkEntity.networkServer.Server.Tick(100);
                    }
                }
            }
        }
    }
}