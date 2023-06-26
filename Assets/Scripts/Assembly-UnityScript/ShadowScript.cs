using System;
using UnityEngine;

[Serializable]
public class ShadowScript : MonoBehaviour
{
	public Transform Foot;

	public virtual void Update()
	{
		float x = Foot.position.x;
		Vector3 position = transform.position;
		float num = (position.x = x);
		Vector3 vector2 = (transform.position = position);
		float z = Foot.position.z;
		Vector3 position2 = transform.position;
		float num2 = (position2.z = z);
		Vector3 vector4 = (transform.position = position2);
	}

	public virtual void Main()
	{
	}
}
