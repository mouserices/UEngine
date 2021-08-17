using GraphProcessor;
using UEngine.NP;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[CustomEditor(typeof(GraphDebugger))]
public class GraphDebuggerEditor : Editor
{
    GraphDebugger behaviour => target as GraphDebugger;

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();

        root.Add(new Button(() => OpenGraph(behaviour.NpBehaveName,behaviour.gameObject.scene))
        {
            text = "Open"
        });

        return root;
    }

    public bool OpenGraph(string npBehaveName,Scene scene)
    {
        var loadAssetAtPath =
            AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UEngine.NP/Resources/NP_TreeDataConfig.prefab");
        var npTreeDataConfig = loadAssetAtPath.GetComponent<NP_TreeDataConfig>();
        var graphPath = npTreeDataConfig.NP_TreeDatasToGraphPath[npBehaveName];

        BaseGraph baseGraph = AssetDatabase.LoadAssetAtPath<BaseGraph>(graphPath);
        baseGraph.LinkToScene(scene);
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
                npBehaveGraphWindow.SetGraphDebugger(behaviour);
                npBehaveGraphWindow.InitializeGraph(npBehaveGraph);
                break;
            default:
                break;
        }

        return true;
    }
}