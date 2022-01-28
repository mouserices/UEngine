//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 8:06:52
//------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;

namespace UEngine.NP
{
    /// <summary>
    /// 技能配置数据载体
    /// </summary>
    [HideLabel]
    public class NP_DataSupportor
    {
        // [BoxGroup("技能中的Buff数据结点")]
        // [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        // public Dictionary<long, BuffNodeDataBase> BuffNodeDataDic = new Dictionary<long, BuffNodeDataBase>();

        [HideInInspector]
        public NP_DataSupportorBase NpDataSupportorBase = new NP_DataSupportorBase();
    }
}