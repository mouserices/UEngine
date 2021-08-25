using System.Collections.Generic;
using Leopotam.Ecs;
using NPBehave;
using UEngine.NP.Component;
using UEngine.NP.Component.Event;
using UEngine.NP.Unit;
using UnityEngine;
using Exception = System.Exception;

namespace UEngine.NP
{
    public class NP_TreeFactorySystem : IEcsRunSystem
    {
        private EcsFilter<RequestRunNpEvent, NP_TreeComponent,WrapperUnityObjectComponent<BaseUnit>> EcsFilter;

        public void Run()
        {
            foreach (var i in EcsFilter)
            {
                ref var requestRunNpEvent = ref EcsFilter.Get1(i);
                ref var npTreeComponent = ref EcsFilter.Get2(i);
                ref var wrapperUnityObjectComponent = ref EcsFilter.Get3(i);
                foreach (string npBehave in requestRunNpEvent.NPBehaves)
                {
                    CreateNPTree(npBehave, ref npTreeComponent,wrapperUnityObjectComponent.Value);
                }
            }
        }

        private void CreateNPTree(string npTreeName, ref NP_TreeComponent npTreeComponent,BaseUnit baseUnit)
        {
            ref var npTreeDataComponent = ref Game.MainEntity.Get<NP_TreeDataComponent>();
            var npDataSupportorBase = npTreeDataComponent.NpDataSupportorBases[npTreeName];
            long rootId = npDataSupportorBase.NPBehaveTreeDataId;
            NP_RuntimeTree npRuntimeTree = new NP_RuntimeTree();
#if UNITY_EDITOR
            GameObject o = new GameObject();
            o.name = $"~{npTreeName}";
            var npBehaveStateSearcher = o.AddComponent<GraphDebugger>();
            npBehaveStateSearcher.NpBehaveName = npTreeName;
#endif

            foreach (var nodeDateBase in npDataSupportorBase.NP_DataSupportorDic)
            {
                switch (nodeDateBase.Value.NodeType)
                {
                    case NodeType.Task:
                        try
                        {
                            nodeDateBase.Value.CreateTask(npRuntimeTree,baseUnit);
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
                                npDataSupportorBase.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]].NP_GetNode(),npRuntimeTree);
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
            if (npTreeComponent.Roots == null)
            {
                npTreeComponent.Roots = new List<Root>();
            }
            npTreeComponent.Roots.Add(root);
            root.Start();
        }
    }
}