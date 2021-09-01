using System.Collections.Generic;
using System.IO;
using Entitas;
using MongoDB.Bson.Serialization;
using NPBehave;
using UnityEngine;
using Exception = System.Exception;

namespace UEngine.NP
{
    public class BehaveTreeFactorySystem : ReactiveSystem<GameEntity>, IInitializeSystem
    {
        private Contexts m_Contexts;

        public BehaveTreeFactorySystem(Contexts contexts) : base(contexts.game)
        {
            m_Contexts = contexts;
        }

        public void Initialize()
        {
            Dictionary<string, NP_DataSupportorBase> BehaveTreeDatas = new Dictionary<string, NP_DataSupportorBase>();
            var npTreeDataConfig = Resources.Load<NPBehaveConfigs>("NPBehaveConfigs");

            foreach (var npTreeData in npTreeDataConfig.Configs)
            {
                if (!BehaveTreeDatas.ContainsKey(npTreeData.Key))
                {
                    NP_DataSupportorBase npDataSupportorBase =
                        BsonSerializer.Deserialize<NP_DataSupportorBase>(npTreeData.Value.TextAsset.bytes);
                    BehaveTreeDatas.Add(npTreeData.Key, npDataSupportorBase);
                }
            }

            m_Contexts.game.SetBehaveTreeData(BehaveTreeDatas);
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.BehaveTreeLoad);
        }

        protected override bool Filter(GameEntity entity) => entity.hasBehaveTree;


        protected override void Execute(List<GameEntity> entities)
        {
            foreach (GameEntity entity in entities)
            {
                LoadBehaveTree(entity);
            }
        }

        private void LoadBehaveTree(GameEntity entity)
        {
            var behaveTreeLoadComponent = entity.behaveTreeLoad;
            foreach (string behaveTreeName in behaveTreeLoadComponent.BehaveTreeNames)
            {
                CreateNPTree(entity, behaveTreeName);
            }
        }
       
        private void CreateNPTree(GameEntity entity, string behaveTreeName)
        {
            var npDataSupportorBases = m_Contexts.game.behaveTreeDataEntity.behaveTreeData.BehaveTreeDatas;

            var npDataSupportorBase = npDataSupportorBases[behaveTreeName];
            long rootId = npDataSupportorBase.NPBehaveTreeDataId;
            NP_RuntimeTree npRuntimeTree = new NP_RuntimeTree();
#if UNITY_EDITOR
            GameObject o = new GameObject();
            o.name = $"~{behaveTreeName}";
            var npBehaveStateSearcher = o.AddComponent<GraphDebugger>();
            npBehaveStateSearcher.NpBehaveName = behaveTreeName;
#endif

            foreach (var nodeDateBase in npDataSupportorBase.NP_DataSupportorDic)
            {
                switch (nodeDateBase.Value.NodeType)
                {
                    case NodeType.Task:
                        try
                        {
                            nodeDateBase.Value.CreateTask(npRuntimeTree, entity.unit.ID);
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
                                npDataSupportorBase.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]].NP_GetNode(),
                                npRuntimeTree);
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
            npBehaveStateSearcher.AddNode(rootId, root);
#endif
            entity.behaveTree.BehaveTreeRoots.Add(root);
            root.Start();
        }
    }
}