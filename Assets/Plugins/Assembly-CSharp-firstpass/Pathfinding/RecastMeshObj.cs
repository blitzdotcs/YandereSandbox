using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Navmesh/RecastMeshObj")]
	public class RecastMeshObj : MonoBehaviour
	{
		protected static RecastBBTree tree = new RecastBBTree();

		protected static List<RecastMeshObj> dynamicMeshObjs = new List<RecastMeshObj>();

		[HideInInspector]
		public Bounds bounds;

		public bool dynamic;

		public int area;

		private bool _dynamic;

		private bool registered;

		public static void GetAllInBounds(List<RecastMeshObj> buffer, Bounds bounds)
		{
			if (!Application.isPlaying)
			{
				RecastMeshObj[] array = UnityEngine.Object.FindObjectsOfType(typeof(RecastMeshObj)) as RecastMeshObj[];
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RecalculateBounds();
					if (array[i].GetBounds().Intersects(bounds))
					{
						buffer.Add(array[i]);
					}
				}
				return;
			}
			if (Time.timeSinceLevelLoad == 0f)
			{
				RecastMeshObj[] array2 = UnityEngine.Object.FindObjectsOfType(typeof(RecastMeshObj)) as RecastMeshObj[];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].Register();
				}
			}
			for (int k = 0; k < dynamicMeshObjs.Count; k++)
			{
				if (dynamicMeshObjs[k].GetBounds().Intersects(bounds))
				{
					buffer.Add(dynamicMeshObjs[k]);
				}
			}
			Rect rect = Rect.MinMaxRect(bounds.min.x, bounds.min.z, bounds.max.x, bounds.max.z);
			tree.QueryInBounds(rect, buffer);
		}

		private void OnEnable()
		{
			Register();
		}

		private void Register()
		{
			if (!registered)
			{
				registered = true;
				area = Mathf.Clamp(area, -1, 33554432);
				Renderer renderer = base.GetComponent<Renderer>();
				Collider collider = base.GetComponent<Collider>();
				if (renderer == null && collider == null)
				{
					throw new Exception("A renderer or a collider should be attached to the GameObject");
				}
				MeshFilter component = GetComponent<MeshFilter>();
				if (renderer != null && component == null)
				{
					throw new Exception("A renderer was attached but no mesh filter");
				}
				if (renderer != null)
				{
					bounds = renderer.bounds;
				}
				else
				{
					bounds = collider.bounds;
				}
				_dynamic = dynamic;
				if (_dynamic)
				{
					dynamicMeshObjs.Add(this);
				}
				else
				{
					tree.Insert(this);
				}
			}
		}

		private void RecalculateBounds()
		{
			Renderer renderer = base.GetComponent<Renderer>();
			Collider collider = GetCollider();
			if (renderer == null && collider == null)
			{
				throw new Exception("A renderer or a collider should be attached to the GameObject");
			}
			MeshFilter component = GetComponent<MeshFilter>();
			if (renderer != null && component == null)
			{
				throw new Exception("A renderer was attached but no mesh filter");
			}
			if (renderer != null)
			{
				bounds = renderer.bounds;
			}
			else
			{
				bounds = collider.bounds;
			}
		}

		public Bounds GetBounds()
		{
			if (_dynamic)
			{
				RecalculateBounds();
			}
			return bounds;
		}

		public MeshFilter GetMeshFilter()
		{
			return GetComponent<MeshFilter>();
		}

		public Collider GetCollider()
		{
			return base.GetComponent<Collider>();
		}

		private void OnDisable()
		{
			registered = false;
			if (_dynamic)
			{
				dynamicMeshObjs.Remove(this);
			}
			else if (!tree.Remove(this))
			{
				throw new Exception("Could not remove RecastMeshObj from tree even though it should exist in it. Has the object moved without being marked as dynamic?");
			}
			_dynamic = dynamic;
		}
	}
}
