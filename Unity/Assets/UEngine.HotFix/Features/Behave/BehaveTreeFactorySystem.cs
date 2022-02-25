using System.Collections.Generic;
using System.IO;
using Entitas;
using Newtonsoft.Json;
using NPBehave;
using UnityEngine;
using Exception = System.Exception;

namespace UEngine.NP
{
    public class BehaveTreeFactorySystem : ReactiveSystem<UnitEntity>, IInitializeSystem
    {
        private Contexts m_Contexts;

        public BehaveTreeFactorySystem(Contexts contexts) : base(contexts.unit)
        {
            m_Contexts = contexts;
        }

        public void Initialize()
        {
            string npBehaveConfigsPath = string.Empty;
            var runPlatform = RunPlatform.Client;
#if CLIENT
            npBehaveConfigsPath = $"{UnityEngine.Application.dataPath}/Res/Config/Bytes";
            runPlatform = RunPlatform.Client;
#elif SERVER
            npBehaveConfigsPath = "../../../../Config";
            runPlatform = RunPlatform.Server;
#endif

            Dictionary<int, NP_DataSupportorBase> BehaveTreeDatas = new Dictionary<int, NP_DataSupportorBase>();

            var filePaths = Directory.GetFiles(npBehaveConfigsPath, "*.json");
            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);

                var readAllBytes = File.ReadAllText(filePath);
                // NP_DataSupportorBase npDataSupportorBase =
                //     BsonSerializer.Deserialize<NP_DataSupportorBase>(readAllBytes);
                
                var jsonSerializerSettings = new JsonSerializerSettings();
                jsonSerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                Debug.Log("11111");
                NP_DataSupportorBase npDataSupportorBase = JsonConvert.DeserializeObject<NP_DataSupportorBase>(readAllBytes,jsonSerializerSettings);
                Debug.Log("2222");

                if (npDataSupportorBase.Platform == runPlatform)
                {
                    BehaveTreeDatas.Add(npDataSupportorBase.SkillID, npDataSupportorBase);
                }
            }

            m_Contexts.game.SetBehaveTreeData(BehaveTreeDatas);
        }

        protected override ICollector<UnitEntity> GetTrigger(IContext<UnitEntity> context)
        {
            return context.CreateCollector(UnitMatcher.BehaveTreeLoad);
        }

        protected override bool Filter(UnitEntity entity)
        {
            return entity.hasBehaveTreeLoad && entity.hasSkillContainer && entity.hasUnit;
        }

        protected override void Execute(List<UnitEntity> entities)
        {
            foreach (UnitEntity entity in entities)
            {
                LoadBehaveTree(entity);
            }
        }

        private void LoadBehaveTree(UnitEntity entity)
        {
            var behaveTreeLoadComponent = entity.behaveTreeLoad;
            foreach (int skillID in behaveTreeLoadComponent.SkillIDs)
            {
                CreateNPTree(entity, skillID);
            }
        }

        private void CreateNPTree(UnitEntity entity, int skillID)
        {
            var npDataSupportorBases = m_Contexts.game.behaveTreeDataEntity.behaveTreeData.NameToBehaveTreeDatas;

            if (!npDataSupportorBases.ContainsKey(skillID))
            {
                MetaContext.Get<ILogService>().LogError($"can not find behaveTreeData,skillID: {skillID}");
                return;
            }
            var npDataSupportorBase = npDataSupportorBases[skillID];
            long rootId = npDataSupportorBase.NPBehaveTreeDataId;
            Skill skill = new Skill();
            skill.ID = npDataSupportorBase.SkillID;
            skill.UnitID = entity.unit.ID;
#if UNITY_EDITOR
            // GameObject o = new GameObject();
            // o.name = $"~{behaveTreeName}";
            // var npBehaveStateSearcher = o.AddComponent<GraphDebugger>();
            // npBehaveStateSearcher.NpBehaveName = behaveTreeName;
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
                            // npBehaveStateSearcher.AddNode(nodeDateBase.Value.LinkedIds[0],
                            //     npDataSupportorBase.NP_DataSupportorDic[nodeDateBase.Value.LinkedIds[0]].NP_GetNode());
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
                                // npBehaveStateSearcher.AddNode(npDataSupportorBase.NP_DataSupportorDic[linkedId].id,
                                //     npDataSupportorBase.NP_DataSupportorDic[linkedId].NP_GetNode());
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
                    //buffData.Value.BehavetreeName = behaveTreeName;
                }
            }

#if UNITY_EDITOR
            // npBehaveStateSearcher.AddNode(rootId, root);
#endif
            entity.skillContainer.Skills.Add(skill);
            root.Start();
        }
    }
}