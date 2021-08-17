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

            var npTreeDataConfig = ResKit.Load<NPBehaveConfigs>("NPBehaveConfigs");
            foreach (var npTreeData in npTreeDataConfig.Configs)
            {
                if (!npTreeDataComponent.NpDataSupportorBases.ContainsKey(npTreeData.Key))
                { NP_DataSupportorBase npDataSupportorBase =
                                         BsonSerializer.Deserialize<NP_DataSupportorBase>(npTreeData.Value.TextAsset.bytes);
                                     npTreeDataComponent.NpDataSupportorBases.Add(npTreeData.Key, npDataSupportorBase);
                   
                }
            }
        }
    }
}