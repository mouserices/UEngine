using System.Collections.Generic;
using GraphProcessor;
using UEngine.NP;
using UEngine.NP.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public class NPBehaveGraphWindow : UniversalGraphWindow
{
    private GraphDebugger m_NpBehaveStateSearcher;
    private Dictionary<long, int> m_debugStartCalls;

    protected override void InitializeWindow(BaseGraph graph)
    {
        graphView = new UniversalGraphView(this);

        m_MiniMap = new MiniMap() {anchored = true};
        graphView.Add(m_MiniMap);

        m_ToolbarView = new NPBehaveToolbarView(this,graphView, m_MiniMap, graph);
        graphView.Add(m_ToolbarView);

        this.SetCurrentBlackBoardDataManager();

        if (m_NpBehaveStateSearcher != null)
        {
            m_debugStartCalls = new Dictionary<long, int>();
            graphView.initialized += () =>
            {
                EnalbeFlowPoint();
                UpdateNodeViewDebug();
            };
        }
    }

    private void OnFocus()
    {
        SetCurrentBlackBoardDataManager();
    }

    private void SetCurrentBlackBoardDataManager()
    {
        NPBehaveGraph npBehaveGraph = (this.graph as NPBehaveGraph);

        if (npBehaveGraph == null)
        {
            //因为OnFocus执行时机比较诡异，在OnEnable后，或者执行一些操作后都会执行，但这时Graph可能为空，所以做判断
            return;
        }

        NP_BlackBoardDataManager.CurrentEditedNP_BlackBoardDataManager =
            (this.graph as NPBehaveGraph).NpBlackBoardDataManager;
    }

    private void UpdateNodeViewDebug()
    {
        var graphViewNodeViews = graphView.nodeViews;
        foreach (var nodeView in graphViewNodeViews)
        {
            var npNodeBase = nodeView.nodeTarget as NP_NodeBase;
            var id = npNodeBase.NP_GetNodeData().id;

            Label label = new Label();
            nodeView.contentContainer.Add(label);

            nodeView.schedule.Execute(() =>
            {
                if (m_NpBehaveStateSearcher == null)
                {
                    return;
                }

                var debugNumStartCalls = m_NpBehaveStateSearcher.GetDebugNumStartCalls(id);
                label.text = debugNumStartCalls.ToString();
            }).Every(100);
        }
    }

    public void EnalbeFlowPoint()
    {
        var graphViewEdgeViews = graphView.edgeViews;
        foreach (var edgeView in graphViewEdgeViews)
        {
            var npNodeView = (NP_NodeView) edgeView.input.node;
            var npNodeBase = npNodeView.nodeTarget as NP_NodeBase;
            var id = npNodeBase.NP_GetNodeData().id;
            
            edgeView.EnableFlowPoint(() =>
            {
                if (m_NpBehaveStateSearcher == null)
                {
                    return;
                }
                m_debugStartCalls[id] = m_NpBehaveStateSearcher.GetDebugNumStartCalls(id);

            }, () =>
            {
                if (m_NpBehaveStateSearcher == null)
                {
                    return false;
                }

                if (!m_debugStartCalls.ContainsKey(id))
                {
                    m_debugStartCalls.Add(id,0);
                }

                var checkNeedEdge = m_NpBehaveStateSearcher.CheckNeedEdge(id, m_debugStartCalls[id]);

                return checkNeedEdge;
            });
        }
    }

    public void SetGraphDebugger(GraphDebugger npBehaveStateSearcher)
    {
        m_NpBehaveStateSearcher = npBehaveStateSearcher;
    }
}