using System.Collections.Generic;
using Leopotam.Ecs;
using NPBehave;
using UEngine.NP.Component;
using UEngine.NP.Component.Event;
using UnityEngine;
using Exception = System.Exception;

namespace UEngine.NP
{
    public class NP_TreeFactorySystem : IEcsRunSystem
    {
        private EcsFilter<RequestRunNpEvent, NP_TreeComponent> EcsFilter;

        public void Run()
        {
            foreach (var i in EcsFilter)
            {
                ref var requestRunNpEvent = ref EcsFilter.Get1(i);
                ref var npTreeComponent = ref EcsFilter.Get2(i);
                CreateNPTree(requestRunNpEvent.NP_TreeName, ref npTreeComponent);
            }
        }

        private void CreateNPTree(string npTreeName, ref NP_TreeComponent npTreeComponent)
        {
            ref var npTreeDataComponent = ref Game.MainEntity.Get<NP_TreeDataComponent>();
            var npDataSupportorBase = npTreeDataComponent.NpDataSupportorBases[npTreeName];
            long rootId = npDataSupportorBase.NPBehaveTreeDataId;
            NP_RuntimeTree npRuntimeTree = new NP_RuntimeTree();
#if UNITY_EDITOR
            GameObject o = new GameObject();
            o.name = $"~{npTreeName}";
            var npBehaveStateSearcher = o.AddComponent<GraphDebugger>();
            npBehaveStateSearcher.InstanceID = npDataSupportorBase.InstanceID;
#endif

            foreach (var nodeDateBase in npDataSupportorBase.NP_DataSupportorDic)
            {
                switch (nodeDateBase.Value.NodeType)
                {
                    case NodeType.Task:
                        try
                        {
                            nodeDateBase.Value.CreateTask(npRuntimeTree);
                        }
                        catch (Exception e)
                        {
                            throw;
                        }

                        break;
                    case NodeType.Decorator:
                        try
                        {
                            nodeDateBase.Value.CreateDecoratorNode(
                                npDataSupportorBase.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]].NP_GetNode());
#if UNITY_EDITOR
                            npBehaveStateSearcher.AddNode(nodeDateBase.Value.LinkedIds[0],
                                npDataSupportorBase.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]].NP_GetNode());
#endif
                        }
                        catch (Exception e)
                        {
                            throw;
                        }

                        break;
                    case NodeType.Composite:
                        try
                        {
                            List<Node> temp = new List<Node>();
                            foreach (var linkedId in nodeDateBase.Value.LinkedIds)
                            {
                                temp.Add(npDataSupportorBase.NP_DataSupportorDic[linkedId].NP_GetNode());

#if UNITY_EDITOR
                                npBehaveStateSearcher.AddNode(npDataSupportorBase.NP_DataSupportorDic[linkedId].id,
                                    npDataSupportorBase.NP_DataSupportorDic[linkedId].NP_GetNode());
#endif
                            }

                            nodeDateBase.Value.CreateComposite(temp.ToArray());
                        }
                        catch (Exception e)
                        {
                            throw;
                        }

                        break;
                }
            }

            

            //配置黑板数据
            Root root = npDataSupportorBase.NP_DataSupportorDic[rootId].NP_GetNode() as Root;
            npRuntimeTree.Blackboard = root.blackboard;
            Dictionary<string, ANP_BBValue> bbvaluesManager = root.blackboard.GetDatas();
            foreach (var bbValues in npDataSupportorBase.NP_BBValueManager)
            {
                bbvaluesManager.Add(bbValues.Key, bbValues.Value);
            }
            
#if UNITY_EDITOR
            npBehaveStateSearcher.AddNode(rootId,root);
#endif

            npTreeComponent.Root = root;
            npTreeComponent.Root.Start();
        }
    }
}