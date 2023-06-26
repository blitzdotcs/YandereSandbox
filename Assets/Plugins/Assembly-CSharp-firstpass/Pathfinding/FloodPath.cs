using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	public class FloodPath : Path
	{
		public Vector3 originalStartPoint;

		public Vector3 startPoint;

		public GraphNode startNode;

		protected Dictionary<GraphNode, GraphNode> parents;

		[Obsolete("Please use the Construct method instead")]
		public FloodPath(Vector3 start, OnPathDelegate callbackDelegate)
		{
			Setup(start, callbackDelegate);
			heuristic = Heuristic.None;
		}

		public FloodPath()
		{
		}

		public bool HasPathTo(GraphNode node)
		{
			return parents != null && parents.ContainsKey(node);
		}

		public GraphNode GetParent(GraphNode node)
		{
			return parents[node];
		}

		public static FloodPath Construct(Vector3 start, OnPathDelegate callback = null)
		{
			FloodPath floodPath = PathPool<FloodPath>.GetPath();
			floodPath.Setup(start, callback);
			return floodPath;
		}

		protected void Setup(Vector3 start, OnPathDelegate callback)
		{
			base.callback = callback;
			originalStartPoint = start;
			startPoint = start;
			heuristic = Heuristic.None;
		}

		public override void Reset()
		{
			base.Reset();
			originalStartPoint = Vector3.zero;
			startPoint = Vector3.zero;
			startNode = null;
			parents = new Dictionary<GraphNode, GraphNode>();
		}

		protected override void Recycle()
		{
			PathPool<FloodPath>.Recycle(this);
		}

		public override void Prepare()
		{
			nnConstraint.tags = enabledTags;
			NNInfo nearest = AstarPath.active.GetNearest(originalStartPoint, nnConstraint);
			startPoint = nearest.clampedPosition;
			startNode = nearest.node;
			if (startNode == null)
			{
				Error();
				LogError("Couldn't find a close node to the start point");
			}
			else if (!startNode.Walkable)
			{
				Error();
				LogError("The node closest to the start point is not walkable");
			}
		}

		public override void Initialize()
		{
			PathNode pathNode = pathHandler.GetPathNode(startNode);
			pathNode.node = startNode;
			pathNode.pathID = pathHandler.PathID;
			pathNode.parent = null;
			pathNode.cost = 0u;
			pathNode.G = GetTraversalCost(startNode);
			pathNode.H = CalculateHScore(startNode);
			parents[startNode] = null;
			startNode.Open(this, pathNode, pathHandler);
			searchedNodes++;
			if (pathHandler.HeapEmpty())
			{
				base.CompleteState = PathCompleteState.Complete;
			}
			currentR = pathHandler.PopNode();
		}

		public override void CalculateStep(long targetTick)
		{
			int num = 0;
			while (base.CompleteState == PathCompleteState.NotCalculated)
			{
				searchedNodes++;
				currentR.node.Open(this, currentR, pathHandler);
				parents[currentR.node] = currentR.parent.node;
				if (pathHandler.HeapEmpty())
				{
					base.CompleteState = PathCompleteState.Complete;
					break;
				}
				currentR = pathHandler.PopNode();
				if (num > 500)
				{
					if (DateTime.UtcNow.Ticks >= targetTick)
					{
						break;
					}
					num = 0;
					if (searchedNodes > 1000000)
					{
						throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
					}
				}
				num++;
			}
		}
	}
}
