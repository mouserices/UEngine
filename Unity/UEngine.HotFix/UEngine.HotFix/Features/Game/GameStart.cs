namespace UEngine
{
    public class GameStart
    {
        private static ClientSystemsConfiguration _clientSystemsConfiguration;
        public static void Start()
        {
            UnityEngine.Debug.LogError("GameStart.Start");
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
        public void OnDestroy()
        {
            // _clientSystemsConfiguration.TearDown();
        }
    }
}

