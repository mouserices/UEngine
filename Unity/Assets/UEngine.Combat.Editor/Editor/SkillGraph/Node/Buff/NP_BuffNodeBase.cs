using GraphProcessor;
using UnityEngine;


    public class NP_BuffNodeBase: BaseNode
    {
        [Input("InputBuff", allowMultiple = true)]
        [HideInInspector]
        public NP_BuffNodeBase PrevNode;
        
        [Output("OutputBuff", allowMultiple = true)]
        [HideInInspector]
        public NP_BuffNodeBase NextNode;

        public override Color color => Color.green;

        public virtual void AutoAddLinkedBuffs()
        {
            
        }
        
        public virtual NP_BuffNodeDataBase GetBuffNodeData()
        {
            return null;
        }
    }
