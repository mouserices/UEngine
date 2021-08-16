using System.Collections.Generic;
using GraphProcessor;
using NPBehave;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace UEngine.NP
{
    public class GraphDebugger : MonoBehaviour
    {
        private string npBehaveName;

        public string NpBehaveName
        {
            get => npBehaveName;
            set => npBehaveName = value;
        }

        private Dictionary<long, Node> m_Nodes = new Dictionary<long, Node>();
        

        public bool CheckNeedEdge(long id,int num)
        {
            if (!m_Nodes.ContainsKey(id))
            {
                return false;
            }
            
           return num < m_Nodes[id].DebugNumStartCalls;
        }

        public int GetDebugNumStartCalls(long id)
        {
            if (!m_Nodes.ContainsKey(id))
            {
                return 0;
            }

            return m_Nodes[id].DebugNumStartCalls;
        }

        public void AddNode(long id, Node node)
        {
            if (m_Nodes.ContainsKey(id))
            {
                return;
            }

            m_Nodes.Add(id, node);
        }
    }
}