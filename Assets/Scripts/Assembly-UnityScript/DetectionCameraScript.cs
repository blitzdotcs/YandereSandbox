using System;
using UnityEngine;

[Serializable]
public class DetectionCameraScript : MonoBehaviour
{
	public Transform YandereChan;

	public virtual void Update()
	{
		transform.position = YandereChan.transform.position + Vector3.up * 100f;
		int num = 90;
		Vector3 eulerAngles = transform.eulerAngles;
		float num2 = (eulerAngles.x = num);
		Vector3 vector2 = (transform.eulerAngles = eulerAngles);
	}

	public virtual void Main()
	{
	}
}
