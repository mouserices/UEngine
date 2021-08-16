using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace GraphProcessor
{
	public class EdgeView : Edge
	{
		public bool					isConnected = false;

		public SerializableEdge		serializedEdge { get { return userData as SerializableEdge; } }

		readonly string				edgeStyle = "GraphProcessorStyles/EdgeView";

		protected BaseGraphView		owner => ((input ?? output) as PortView).owner.owner;

		public Vector2[] GetPointsAndTangents => PointsAndTangents;

		public List<VisualElement> EdgeFlowPointVisualElements;
		
		public List<float> FlowPointProgress = new List<float>();

		private FlowPoint flowPoint;

		public EdgeView() : base()
		{
			styleSheets.Add(Resources.Load<StyleSheet>(edgeStyle));
			RegisterCallback<MouseDownEvent>(OnMouseDown);
		}

		public void EnableFlowPoint(Action finish = null,Func<bool> isConditionMet = null)
		{
			flowPoint = new FlowPoint();
			flowPoint.Finish = finish;
			flowPoint.IsConditionMet = isConditionMet;
			this.AddManipulator(flowPoint);
		}

		public void DisalbeFlowPoint()
		{
			if (flowPoint == null)
			{
				return;
			}
			this.RemoveManipulator(flowPoint);
		}

		public override void OnPortChanged(bool isInput)
		{
			base.OnPortChanged(isInput);
			UpdateEdgeSize();
		}

		public void UpdateEdgeSize()
		{
			if (input == null && output == null)
				return;

			PortData inputPortData = (input as PortView)?.portData;
			PortData outputPortData = (output as PortView)?.portData;

			for (int i = 1; i < 20; i++)
				RemoveFromClassList($"edge_{i}");
			int maxPortSize = Mathf.Max(inputPortData?.sizeInPixel ?? 0, outputPortData?.sizeInPixel ?? 0);
			if (maxPortSize > 0)
				AddToClassList($"edge_{Mathf.Max(1, maxPortSize - 6)}");
		}

		protected override void OnCustomStyleResolved(ICustomStyle styles)
		{
			base.OnCustomStyleResolved(styles);

			UpdateEdgeControl();
		}

		void OnMouseDown(MouseDownEvent e)
		{
			if (e.clickCount == 2)
			{
				// Empirical offset:
				var position = e.mousePosition;
                position += new Vector2(-10f, -28);
                Vector2 mousePos = owner.ChangeCoordinatesTo(owner.contentViewContainer, position);

				owner.AddRelayNode(input as PortView, output as PortView, mousePos);
			}
		}
	}
	
	public class FlowPoint : Manipulator
	{
		VisualElement point { get; set; }

		public Action Finish;
		public Func<bool> IsConditionMet;

		protected override void RegisterCallbacksOnTarget()
		{
			if (target is Edge edge)
			{
				point = new VisualElement();
				point.AddToClassList("flow-point");
				point.style.left = 0;
				point.style.top = 0;
				target.Add(point);

				target.schedule.Execute(() =>
				{
					if (IsConditionMet())
					{
						UpdateCapPoint(edge, (float)(EditorApplication.timeSinceStartup % 1 / 1));
					}
					else
					{
						point.style.left = 0;
						point.style.top = 0;
					}
				}).Until(() => point == null);
			}
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			if (point != null)
			{
				target.Remove(point);
				point = null;
			}
		}

		public void UpdateCapPoint(Edge _edge, float _t)
		{
			if (point == null)
			{
				return;
			}
			Vector2 v = Lerp(_edge.edgeControl.controlPoints, _t);
			point.style.left = v.x;
			point.style.top = v.y;

			Vector2 lastPoint = _edge.edgeControl.controlPoints[_edge.edgeControl.controlPoints.Length - 1];
			if (1- _t < 0.8f)
			{
				if (Finish != null)
				{
					Finish();
					//Finish = null;
				}
			}
		}

		Vector2 Lerp(Vector2[] points, float t)
		{
			t = Mathf.Clamp01(t);
			float totalLength = 0;
			for (int i = 0; i < points.Length - 1; i++)
			{
				totalLength += Vector2.Distance(points[i], points[i + 1]);
			}

			float pointLength = Mathf.Lerp(0, totalLength, t);

			float tempLength = 0;
			for (int i = 0; i < points.Length - 1; i++)
			{
				float d = Vector2.Distance(points[i], points[i + 1]);
				if (pointLength <= tempLength + d)
					return Vector2.Lerp(points[i], points[i + 1], (pointLength - tempLength) / d);
				tempLength += d;
			}
			return points[0];
		}
	}
}