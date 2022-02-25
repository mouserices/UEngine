using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using ProtoBuf;
using UEngine.Net;
using UnityEngine;

namespace UEngine
{
    public class GameStart
    {
        private static ClientSystemsConfiguration _clientSystemsConfiguration;
        public static void Start()
        {
            Init();
            _clientSystemsConfiguration = new ClientSystemsConfiguration();
            _clientSystemsConfiguration.InitializeSystems(Contexts.sharedInstance);
            _clientSystemsConfiguration.Initialize();
            
            MetaContext.Get<INetworkService>().StartClient("localhost", 1337);
           
            // using (var stream =File.Open(Application.dataPath + "/ip.txt",FileMode.OpenOrCreate))
            // {
            //     ProtoBuf.Serializer.Serialize(stream,new C2S_Login(){UserName = "zz",Password = "123"});
            //     
            //     Debug.Log("succeed");
            // }
        }

        public static void Update()
        {
            _clientSystemsConfiguration?.Execute();
            _clientSystemsConfiguration?.Cleanup();
        }

        public static void OnDestroy()
        {
            _clientSystemsConfiguration?.TearDown();
        }
        
        public static void Init()
        {
            List<Type> types = Main.GetTypes();

            foreach (Type type in types)
            {
                if (type.GetCustomAttributes(typeof(ProtoContractAttribute), false).Length == 0 &&
                    type.GetCustomAttributes(typeof(ProtoMemberAttribute), false).Length == 0)
                {
                    continue;
                }

                PBType.RegisterType(type.FullName, type);
            }
        }
    }
}