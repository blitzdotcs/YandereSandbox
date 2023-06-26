using System;
using UnityEngine;

[Serializable]
public class DrillScript : MonoBehaviour
{
	public virtual void LateUpdate()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * 3600f);
	}

	public virtual void Main()
	{
	}
}
