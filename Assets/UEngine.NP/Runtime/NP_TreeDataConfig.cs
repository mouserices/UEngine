using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UEngine.NP
{
    
    public class NP_TreeDataConfig : SerializedMonoBehaviour
    {
        public class NP_TreeInfo
        {
            public string NPTreeName;
            public string NPTreeRootID;
        }
        public Dictionary<string, TextAsset> NP_TreeDatas = new Dictionary<string, TextAsset>();
    }
}