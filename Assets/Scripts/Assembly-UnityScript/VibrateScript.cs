using System;
using UnityEngine;

[Serializable]
public class VibrateScript : MonoBehaviour
{
	public Vector3 Origin;

	public virtual void Start()
	{
		Origin = transform.localPosition;
	}

	public virtual void Update()
	{
		float x = Origin.x + UnityEngine.Random.Range(-5f, 5f);
		Vector3 localPosition = transform.localPosition;
		float num = (localPosition.x = x);
		Vector3 vector2 = (transform.localPosition = localPosition);
		float y = Origin.y + UnityEngine.Random.Range(-5f, 5f);
		Vector3 localPosition2 = transform.localPosition;
		float num2 = (localPosition2.y = y);
		Vector3 vector4 = (transform.localPosition = localPosition2);
	}

	public virtual void Main()
	{
	}
}
