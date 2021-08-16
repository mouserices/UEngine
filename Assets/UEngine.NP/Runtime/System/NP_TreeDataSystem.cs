using System.Collections.Generic;
using System.IO;
using Leopotam.Ecs;
using MongoDB.Bson.Serialization;
using UEngine.NP.Component;
using UEngine.UI.Runtime.Tools;
using UnityEngine;

namespace UEngine.NP
{
    public class NP_TreeDataSystem : IEcsInitSystem
    {
        public void Init()
        {
            ref var npTreeDataComponent = ref Game.MainEntity.Get<NP_TreeDataComponent>();
            npTreeDataComponent.NpDataSupportorBases = new Dictionary<string, NP_DataSupportorBase>();

            var gameObject = ResKit.Load<GameObject>("NP_TreeDataConfig");
            var npTreeDataConfig = gameObject.GetComponent<NP_TreeDataConfig>();
            foreach (var npTreeData in npTreeDataConfig.NP_TreeDatasToBytes)
            {
                if (!npTreeDataComponent.NpDataSupportorBases.ContainsKey(npTreeData.Key))
                { NP_DataSupportorBase npDataSupportorBase =
                                         BsonSerializer.Deserialize<NP_DataSupportorBase>(npTreeData.Value.bytes);
                                     npTreeDataComponent.NpDataSupportorBases.Add(npTreeData.Key, npDataSupportorBase);
                   
                }
            }
        }
    }
}