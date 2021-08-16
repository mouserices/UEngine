using GraphProcessor;
using Plugins.Examples.Editor.BaseGraph;
using UEngine.NP;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(GraphDebugger))]
public class GraphDebuggerEditor : Editor
{
    GraphDebugger behaviour => target as GraphDebugger;

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        root.Add(new Button(() => OpenGraph(behaviour.NpBehaveName))
        {
            text = "Open"
        });

        return root;
    }

    public bool OpenGraph(string npBehaveName)
    {
        var loadAssetAtPath =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UEngine.NP/Resources/NP_TreeDataConfig.prefab");
        var npTreeDataConfig = loadAssetAtPath.GetComponent<NP_TreeDataConfig>();
        var graphPath = npTreeDataConfig.NP_TreeDatasToGraphPath[npBehaveName];

        BaseGraph atPath = AssetDatabase.LoadAssetAtPath<BaseGraph>(graphPath);
        return InitializeGraph(atPath);
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
                npBehaveGraphWindow.SetGraphDebugger(behaviour);
                npBehaveGraphWindow.InitializeGraph(npBehaveGraph);
                break;
            default:
                EditorWindow.GetWindow<FallbackGraphWindow>().InitializeGraph(baseGraph);
                break;
        }

        return true;
    }
}