using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace UEngine.NP
{
    public class NPBehaveConfig
    {
        public TextAsset TextAsset;
        public string GraphPath;
    }

    public class NPBehaveConfigs //: SerializedScriptableObject
    {
        [SerializeField]
        public Dictionary<string, NPBehaveConfig> Configs = new Dictionary<string, NPBehaveConfig>();


        public void AddConfig(string npBehaveName,TextAsset textAsset,string graphAssetPath)
        {
            NPBehaveConfig config;
            Configs.TryGetValue(npBehaveName, out config);
            if (config == null)
            {
                config = new NPBehaveConfig();
                Configs.Add(npBehaveName,config);
            }

            config.TextAsset = textAsset;
            config.GraphPath = graphAssetPath;
            
            #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
            #endif
        }
    }
}