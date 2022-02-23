using UnityEngine;

namespace UEngine
{
    public class GameStart
    {
        private static ClientSystemsConfiguration _clientSystemsConfiguration;
        public static void Start()
        {
            _clientSystemsConfiguration = new ClientSystemsConfiguration();
            _clientSystemsConfiguration.InitializeSystems(Contexts.sharedInstance);
            _clientSystemsConfiguration.Initialize();
            
            MetaContext.Get<INetworkService>().StartClient("localhost", 1337);
        }

        public static void Update()
        {
            _clientSystemsConfiguration.Execute();
            _clientSystemsConfiguration.Cleanup();
        }

        public static void OnDestroy()
        {
            _clientSystemsConfiguration.TearDown();
        }
    }
}