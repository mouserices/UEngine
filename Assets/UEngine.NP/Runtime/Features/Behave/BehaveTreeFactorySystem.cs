using System.Collections.Generic;
using System.IO;
using Entitas;
using MongoDB.Bson.Serialization;
using NPBehave;
using UnityEngine;
using Exception = System.Exception;

namespace UEngine.NP
{
    public class BehaveTreeFactorySystem : MultiReactiveSystem<IBehaveTreeLoad,Contexts>, IInitializeSystem
    {
        private Contexts m_Contexts;

        public BehaveTreeFactorySystem(Contexts contexts) : base(contexts)
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

        protected override ICollector[] GetTrigger(Contexts contexts)
        {
            return new ICollector[] {
                contexts.game.CreateCollector(GameMatcher.BehaveTreeLoad),
                contexts.remoteAgent.CreateCollector(RemoteAgentMatcher.BehaveTreeLoad)
            };
        }

        protected override bool Filter(IBehaveTreeLoad entity)
        {
            return entity.hasBehaveTreeLoad && entity.hasSkillContainer && entity.hasUnit;
        }

        protected override void Execute(List<IBehaveTreeLoad> entities)
        {
            foreach (IBehaveTreeLoad entity in entities)
            {
                LoadBehaveTree(entity);
            }
        }

        private void LoadBehaveTree(IBehaveTreeLoad entity)
        {
            var behaveTreeLoadComponent = entity.behaveTreeLoad;
            foreach (string behaveTreeName in behaveTreeLoadComponent.BehaveTreeNames)
            {
                CreateNPTree(entity, behaveTreeName);
            }
        }
       
        private void CreateNPTree(IBehaveTreeLoad entity, string behaveTreeName)
        {
            var npDataSupportorBases = m_Contexts.game.behaveTreeDataEntity.behaveTreeData.NameToBehaveTreeDatas;

            var npDataSupportorBase = npDataSupportorBases[behaveTreeName];
            long rootId = npDataSupportorBase.NPBehaveTreeDataId;
            Skill skill = new Skill();
            skill.ID = npDataSupportorBase.SkillID;
            skill.UnitID = entity.unit.ID;
            skill.NPBehaveName = behaveTreeName;
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
                            nodeDateBase.Value.CreateTask(skill, entity.unit.ID);
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
                                skill);
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
            skill.Blackboard = root.blackboard;
            skill.Root = root;
            Dictionary<string, ANP_BBValue> bbvaluesManager = root.blackboard.GetDatas();
            foreach (var bbValues in npDataSupportorBase.NP_BBValueManager)
            {
                bbvaluesManager.Add(bbValues.Key, bbValues.Value);
            }
            
            //Buff数据
            if (npDataSupportorBase.NP_BuffDatas != null)
            {
                foreach (var buffData in npDataSupportorBase.NP_BuffDatas)
                {
                    buffData.Value.SkillID = skill.ID;
                    buffData.Value.UnitID = skill.UnitID;
                    buffData.Value.BehavetreeName = behaveTreeName;
                }
            }

#if UNITY_EDITOR
            npBehaveStateSearcher.AddNode(rootId, root);
#endif
            entity.skillContainer.Skills.Add(skill);
            root.Start();
        }
    }
}