using UnityEngine;

namespace Pathfinding.RVO
{
	[AddComponentMenu("Pathfinding/Local Avoidance/Square Obstacle (disabled)")]
	public class RVOSquareObstacle : RVOObstacle
	{
		public float height = 1f;

		public Vector2 size = Vector3.one;

		protected override bool StaticObstacle
		{
			get
			{
				return false;
			}
		}

		protected override bool ExecuteInEditor
		{
			get
			{
				return true;
			}
		}

		protected override bool LocalCoordinates
		{
			get
			{
				return true;
			}
		}

		protected override bool AreGizmosDirty()
		{
			return false;
		}

		protected override void CreateObstacles()
		{
			size.x = Mathf.Abs(size.x);
			size.y = Mathf.Abs(size.y);
			height = Mathf.Abs(height);
			Vector3[] array = new Vector3[4]
			{
				new Vector3(1f, 0f, -1f),
				new Vector3(1f, 0f, 1f),
				new Vector3(-1f, 0f, 1f),
				new Vector3(-1f, 0f, -1f)
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Scale(new Vector3(size.x, 0f, size.y));
			}
			AddObstacle(array, height);
		}
	}
}
