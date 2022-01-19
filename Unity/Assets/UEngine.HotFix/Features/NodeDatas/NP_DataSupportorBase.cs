using System.Collections.Generic;
using System.Reflection.Emit;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Sirenix.OdinInspector;

namespace UEngine.NP
{
    [HideLabel]
    public class NP_DataSupportorBase
    {
        [LabelText("技能ID")]public int SkillID;
        public RunPlatform Platform;
        [LabelText("此行为树的InstanceID")]
        public int InstanceID;
        [LabelText("此行为树Id，也是根节点Id")]
        public long NPBehaveTreeDataId;

        [LabelText("单个行为树所有结点")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, NP_NodeDataBase> NP_DataSupportorDic = new Dictionary<long, NP_NodeDataBase>();
        
        [LabelText("黑板数据")]
        public Dictionary<string, ANP_BBValue> NP_BBValueManager = new Dictionary<string, ANP_BBValue>();

        [LabelText("Buff数据")]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<long, NP_BuffNodeDataBase> NP_BuffDatas = new Dictionary<long, NP_BuffNodeDataBase>();
    }
}