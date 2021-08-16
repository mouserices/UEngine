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
        private int instanceID;

        public int InstanceID
        {
            get => instanceID;
            set => instanceID = value;
        }

        private Dictionary<long, Node> m_Nodes = new Dictionary<long, Node>();

        public bool CheckNodeStateActive(long id)
        {
            if (!m_Nodes.ContainsKey(id))
            {
                return false;
            }

            return m_Nodes[id].Excuted;
        }

        public void ResetNode(long id)
        {
            if (!m_Nodes.ContainsKey(id))
            {
                return;
            }

            m_Nodes[id].Excuted = false;
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