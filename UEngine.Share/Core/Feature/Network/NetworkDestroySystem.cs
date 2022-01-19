using Entitas;

namespace UEngine.Net
{
    public class NetworkDestroySystem : ITearDownSystem
    {
        public void TearDown()
        {
            bool hasNetworkClient = Contexts.sharedInstance.network.hasNetworkClient;
            if (hasNetworkClient)
            {
                Contexts.sharedInstance.network.networkClient.Client.Disconnect();
            }

            bool hasNetworkServer = Contexts.sharedInstance.network.hasNetworkServer;
            if (hasNetworkServer)
            {
                Contexts.sharedInstance.network.networkServer.Server.Stop();
            }
        }
    }
}