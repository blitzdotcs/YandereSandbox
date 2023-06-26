using System;
using UnityEngine;

[Serializable]
public class NodeSetterScript : MonoBehaviour
{
	public GameObject[] Nodes;

	public GameObject Node;

	public bool Stairs;

	public bool Door;

	public float Height;

	public int Column;

	public int Row;

	public virtual void Start()
	{
	}

	public virtual void Main()
	{
	}
}
