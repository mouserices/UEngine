using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UnityEditor.Callbacks;
using System.IO;
using UEngine.NP;
using UEngine.NP.Editor;

public class GraphAssetCallbacks
{
	[MenuItem("Assets/Create/GraphProcessor", false, 10)]
	public static void CreateGraphPorcessor()
	{
		var graph = ScriptableObject.CreateInstance< BaseGraph >();
		ProjectWindowUtil.CreateAsset(graph, "GraphProcessor.asset");
	}
	
	[MenuItem("Assets/Create/GraphProcessor_NP", false, 10)]
	public static void CreateGraphPorcessor_NP()
	{
		var graph = ScriptableObject.CreateInstance< NPBehaveGraph >();
		ProjectWindowUtil.CreateAsset(graph, "NPBehaveGraph.asset");
	}
	
	[MenuItem("Assets/Create/GraphProcessor_Skill", false, 10)]
	public static void CreateGraphPorcessor_Skill()
	{
		var graph = ScriptableObject.CreateInstance< SkillGraph >();
		ProjectWindowUtil.CreateAsset(graph, "SkillGraph.asset");
	}
	
	[MenuItem("Assets/Create/NPBehaveConfigs", false, 10)]
	public static void CreateNPBehaveConfigs()
	{
		var graph = ScriptableObject.CreateInstance< NPBehaveConfigs >();
		ProjectWindowUtil.CreateAsset(graph, "NPBehaveConfigs.asset");
	}

	[OnOpenAsset(0)]
	public static bool OnBaseGraphOpened(int instanceID, int line)
	{
		var baseGraph = EditorUtility.InstanceIDToObject(instanceID) as BaseGraph;
		return InitializeGraph(baseGraph);
	}

	public static bool InitializeGraph(BaseGraph baseGraph)
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
				break;
			default:
				break;
		}

		return true;
	}
}
