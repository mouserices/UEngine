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

    public void EnalbeFlowPoint()
    {
        enableFlowPoint = !enableFlowPoint;
        var graphViewEdgeViews = graphView.edgeViews;
        foreach (var edgeView in graphViewEdgeViews)
        {
            if (!checkNodeStateRuntime(edgeView.output.node) || !checkNodeStateRuntime(edgeView.input.node))
            {
                edgeView.DisalbeFlowPoint();
            }
            
            if (enableFlowPoint)
            {
                edgeView.EnableFlowPoint();
            }
            else
            {
                edgeView.DisalbeFlowPoint();
            }
        }
    }

    public void SetNPBehaveStateSearcher(GraphDebugger npBehaveStateSearcher)
    {
        m_NpBehaveStateSearcher = npBehaveStateSearcher;
    }

    private bool checkNodeStateRuntime(Node node)
    {
        if (m_NpBehaveStateSearcher == null)
        {
            return true;
        }
        var npNodeView = (NP_NodeView)node;
        var npNodeBase = npNodeView.nodeTarget as NP_NodeBase;
        var id = npNodeBase.NP_GetNodeData().id;

        return m_NpBehaveStateSearcher.CheckNodeStateActive(id);
    }
}