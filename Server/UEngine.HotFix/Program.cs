using System;
using System.Diagnostics;
using System.Threading;
using UEngine.NP;

namespace UEngine.HotFix
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var serverSystemConfigutation = new ServerSystemConfigutation();
            serverSystemConfigutation.InitializeSystems(Contexts.sharedInstance);
            serverSystemConfigutation.Initialize();


            MetaContext.Get<INetworkService>().StartServer(1337);
            while (true)
            {
                Thread.Sleep(1);
                serverSystemConfigutation.Execute();
                serverSystemConfigutation.Cleanup();
            }
        }
    }
}