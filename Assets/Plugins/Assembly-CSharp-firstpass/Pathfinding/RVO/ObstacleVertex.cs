using UnityEngine;

namespace Pathfinding.RVO
{
	public class ObstacleVertex
	{
		public bool convex;

		public Vector3 position;

		public Vector2 dir;

		public float height;

		public bool split;

		public bool thin;

		public ObstacleVertex next;

		public ObstacleVertex prev;
	}
}
