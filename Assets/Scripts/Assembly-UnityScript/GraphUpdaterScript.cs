using System;
using UnityEngine;

[Serializable]
public class GraphUpdaterScript : MonoBehaviour
{
	public AstarPath Graph;

	public int Frames;

	public virtual void Update()
	{
		if (Frames > 0)
		{
			Graph.Scan();
			UnityEngine.Object.Destroy(this);
		}
		Frames++;
	}

	public virtual void Main()
	{
	}
}
