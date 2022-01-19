namespace UEngine.Net
{
    public static class NetworkHelper
    {
        public static void StartClient(string ip, int port)
        {
            var networkEntity = Contexts.sharedInstance.network.CreateEntity();
            networkEntity.AddClientConnecting(ip, port);
        }
        
        public static void StartServer(int port)
        {
            var networkEntity = Contexts.sharedInstance.network.CreateEntity();
            networkEntity.AddServerListening(port);
        }

        public static void send(int opCode,IMessage message){
            NetMessageEntity netMessageEntity = Contexts.sharedInstance.netMessage.CreateEntity();
            netMessageEntity.AddMessageSend(opCode,message);
        }
    }
}