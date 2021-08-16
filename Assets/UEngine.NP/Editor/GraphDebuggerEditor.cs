using GraphProcessor;
using Plugins.Examples.Editor.BaseGraph;
using UEngine.NP;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(GraphDebugger))]
public class GraphDebuggerEditor : Editor
{
    GraphDebugger behaviour => target as GraphDebugger;

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        root.Add(new Button(() => OpenGraph(behaviour.InstanceID))
        {
            text = "Open"
        });

        return root;
    }

    public bool OpenGraph(int instanceID)
    {
        var baseGraph = EditorUtility.InstanceIDToObject(instanceID) as BaseGraph;
        return InitializeGraph(baseGraph);
    }

    public bool InitializeGraph(BaseGraph baseGraph)
    {
        if (baseGraph == null) return false;
        switch (baseGraph)
        {
            case SkillGraph skillGraph:
                EditorWindow.GetWindow<SkillGraphWindow>().InitializeGraph(skillGraph);
                break;
            case NPBehaveGraph npBehaveGraph:
                var npBehaveGraphWindow = EditorWindow.GetWindow<NPBehaveGraphWindow>();
                npBehaveGraphWindow.InitializeGraph(npBehaveGraph);
                npBehaveGraphWindow.SetGraphDebugger(behaviour);
                break;
            default:
                EditorWindow.GetWindow<FallbackGraphWindow>().InitializeGraph(baseGraph);
                break;
        }

        return true;
    }
}