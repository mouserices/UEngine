using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

public class SkillGraphWindow : UniversalGraphWindow
{
    private GraphDebugger m_NpBehaveStateSearcher;
    private bool enableFlowPoint = false;
    protected override void InitializeWindow(BaseGraph graph)
    {
        graphView = new UniversalGraphView(this);

        m_MiniMap = new MiniMap() {anchored = true};
        graphView.Add(m_MiniMap);

        m_ToolbarView = new SkillToolbarView(this, graphView, m_MiniMap, graph);
        graphView.Add(m_ToolbarView);
    }
}