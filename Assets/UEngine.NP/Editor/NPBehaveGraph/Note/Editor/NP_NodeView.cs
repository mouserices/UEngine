using GraphProcessor;
using UnityEngine.UIElements;

namespace UEngine.NP.Editor
{
    [NodeCustomEditor(typeof(NP_NodeBase))]
    public class NP_NodeView: BaseNodeView
    {
        public override void Enable()
        {
            NP_NodeDataBase nodeDataBase = (this.nodeTarget as NP_NodeBase).NP_GetNodeData();
            TextField textField = new TextField(){ value = nodeDataBase.NodeDes};
            textField.style.marginTop = 4;
            textField.style.marginBottom = 4;
            textField.RegisterValueChangedCallback((changedDes) => { nodeDataBase.NodeDes = changedDes.newValue; });
            controlsContainer.Add(textField);
        }
    }
}