//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月31日 19:15:32
//------------------------------------------------------------

using GraphProcessor;
using Sirenix.OdinInspector;
using UEngine.NP;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


    public class NPBehaveToolbarView: UniversalToolbarView
    {
        private NPBehaveGraphWindow m_NpBehaveGraphWindow;
        public class BlackboardInspectorViewer: SerializedScriptableObject
        {
            public NP_BlackBoardDataManager NpBlackBoardDataManager;
        }

        private static BlackboardInspectorViewer _BlackboardInspectorViewer;

        public static BlackboardInspectorViewer BlackboardInspector
        {
            get
            {
                if (_BlackboardInspectorViewer == null)
                {
                    _BlackboardInspectorViewer = ScriptableObject.CreateInstance<BlackboardInspectorViewer>();
                }

                return _BlackboardInspectorViewer;
            }
        }

        public NPBehaveToolbarView(NPBehaveGraphWindow npBehaveGraphWindow, BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph): base(graphView,
            miniMap, baseGraph)
        {
            m_NpBehaveGraphWindow = npBehaveGraphWindow;
        }

        protected override void AddButtons()
        {
            base.AddButtons();
        
            AddButton(new GUIContent("Blackboard", "打开Blackboard数据面板"),
                () =>
                {
                    NPBehaveToolbarView.BlackboardInspector.NpBlackBoardDataManager =
                            (this.m_BaseGraph as NPBehaveGraph).NpBlackBoardDataManager;
                    Selection.activeObject = BlackboardInspector;
                }, false);
        }
    }
