using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace UEngine.NP
{
    public class NP_TreeDataConfig : SerializedMonoBehaviour
    {
        public Dictionary<string, TextAsset> NP_TreeDatasToBytes = new Dictionary<string, TextAsset>();
        public Dictionary<string, string> NP_TreeDatasToGraphPath = new Dictionary<string, string>();

        public void AddConfig(string npBehaveName,TextAsset textAsset,string graphAssetPath)
        {
            if (NP_TreeDatasToBytes.ContainsKey(npBehaveName))
            {
                NP_TreeDatasToBytes.Remove(npBehaveName);
            }
            
            NP_TreeDatasToBytes.Add(npBehaveName,textAsset);
            
            if (NP_TreeDatasToGraphPath.ContainsKey(npBehaveName))
            {
                NP_TreeDatasToGraphPath.Remove(npBehaveName);
            }
            NP_TreeDatasToGraphPath.Add(npBehaveName,graphAssetPath);
        }
    }
}