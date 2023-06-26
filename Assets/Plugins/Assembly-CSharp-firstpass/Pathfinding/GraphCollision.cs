using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	public class GraphCollision
	{
		public const float RaycastErrorMargin = 0.005f;

		public ColliderType type = ColliderType.Capsule;

		public float diameter = 1f;

		public float height = 2f;

		public float collisionOffset;

		public RayDirection rayDirection = RayDirection.Both;

		public LayerMask mask;

		public LayerMask heightMask = -1;

		public float fromHeight = 100f;

		public bool thickRaycast;

		public float thickRaycastDiameter = 1f;

		public Vector3 up;

		private Vector3 upheight;

		private float finalRadius;

		private float finalRaycastRadius;

		public bool collisionCheck = true;

		public bool heightCheck = true;

		public bool unwalkableWhenNoGround = true;

		public bool use2D;

		public void Initialize(Matrix4x4 matrix, float scale)
		{
			up = matrix.MultiplyVector(Vector3.up);
			upheight = up * height;
			finalRadius = diameter * scale * 0.5f;
			finalRaycastRadius = thickRaycastDiameter * scale * 0.5f;
		}

		public bool Check(Vector3 position)
		{
			if (!collisionCheck)
			{
				return true;
			}
			if (use2D)
			{
				switch (type)
				{
				case ColliderType.Capsule:
					throw new Exception("Capsule mode cannot be used with 2D since capsules don't exist in 2D");
				case ColliderType.Sphere:
					return Physics2D.OverlapCircle(position, finalRadius, mask) == null;
				default:
					return Physics2D.OverlapPoint(position, mask) == null;
				}
			}
			position += up * collisionOffset;
			switch (type)
			{
			case ColliderType.Capsule:
				return !Physics.CheckCapsule(position, position + upheight, finalRadius, mask);
			case ColliderType.Sphere:
				return !Physics.CheckSphere(position, finalRadius, mask);
			default:
				switch (rayDirection)
				{
				case RayDirection.Both:
					return !Physics.Raycast(position, up, height, mask) && !Physics.Raycast(position + upheight, -up, height, mask);
				case RayDirection.Up:
					return !Physics.Raycast(position, up, height, mask);
				default:
					return !Physics.Raycast(position + upheight, -up, height, mask);
				}
			}
		}

		public Vector3 CheckHeight(Vector3 position)
		{
			RaycastHit hit;
			bool walkable;
			return CheckHeight(position, out hit, out walkable);
		}

		public Vector3 CheckHeight(Vector3 position, out RaycastHit hit, out bool walkable)
		{
			walkable = true;
			if (!heightCheck || use2D)
			{
				hit = default(RaycastHit);
				return position;
			}
			if (thickRaycast)
			{
				Ray ray = new Ray(position + up * fromHeight, -up);
				if (Physics.SphereCast(ray, finalRaycastRadius, out hit, fromHeight + 0.005f, heightMask))
				{
					return AstarMath.NearestPoint(ray.origin, ray.origin + ray.direction, hit.point);
				}
				if (unwalkableWhenNoGround)
				{
					walkable = false;
				}
			}
			else
			{
				if (Physics.Raycast(position + up * fromHeight, -up, out hit, fromHeight + 0.005f, heightMask))
				{
					return hit.point;
				}
				if (unwalkableWhenNoGround)
				{
					walkable = false;
				}
			}
			return position;
		}

		public Vector3 Raycast(Vector3 origin, out RaycastHit hit, out bool walkable)
		{
			walkable = true;
			if (!heightCheck || use2D)
			{
				hit = default(RaycastHit);
				return origin - up * fromHeight;
			}
			if (thickRaycast)
			{
				Ray ray = new Ray(origin, -up);
				if (Physics.SphereCast(ray, finalRaycastRadius, out hit, fromHeight + 0.005f, heightMask))
				{
					return AstarMath.NearestPoint(ray.origin, ray.origin + ray.direction, hit.point);
				}
				if (unwalkableWhenNoGround)
				{
					walkable = false;
				}
			}
			else
			{
				if (Physics.Raycast(origin, -up, out hit, fromHeight + 0.005f, heightMask))
				{
					return hit.point;
				}
				if (unwalkableWhenNoGround)
				{
					walkable = false;
				}
			}
			return origin - up * fromHeight;
		}

		public RaycastHit[] CheckHeightAll(Vector3 position)
		{
			if (!heightCheck || use2D)
			{
				RaycastHit raycastHit = default(RaycastHit);
				raycastHit.point = position;
				raycastHit.distance = 0f;
				return new RaycastHit[1] { raycastHit };
			}
			if (thickRaycast)
			{
				Debug.LogWarning("Thick raycast cannot be used with CheckHeightAll. Disabling thick raycast...");
				thickRaycast = false;
			}
			List<RaycastHit> list = new List<RaycastHit>();
			bool walkable = true;
			Vector3 vector = position + up * fromHeight;
			Vector3 vector2 = Vector3.zero;
			int num = 0;
			while (true)
			{
				RaycastHit hit;
				Raycast(vector, out hit, out walkable);
				if (hit.transform == null)
				{
					break;
				}
				if (hit.point != vector2 || list.Count == 0)
				{
					vector = hit.point - up * 0.005f;
					vector2 = hit.point;
					num = 0;
					list.Add(hit);
					continue;
				}
				vector -= up * 0.001f;
				num++;
				if (num <= 10)
				{
					continue;
				}
				Debug.LogError(string.Concat("Infinite Loop when raycasting. Please report this error (arongranberg.com)\n", vector, " : ", vector2));
				break;
			}
			return list.ToArray();
		}
	}
}
