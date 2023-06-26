using System;
using UnityEngine;

[Serializable]
public class DoorBoxScript : MonoBehaviour
{
	public UILabel Label;

	public bool Show;

	public virtual void Update()
	{
		if (Show)
		{
			float y = Mathf.Lerp(transform.localPosition.y, -530f, Time.deltaTime * 10f);
			Vector3 localPosition = transform.localPosition;
			float num = (localPosition.y = y);
			Vector3 vector2 = (transform.localPosition = localPosition);
		}
		else
		{
			float y2 = Mathf.Lerp(transform.localPosition.y, -630f, Time.deltaTime * 10f);
			Vector3 localPosition2 = transform.localPosition;
			float num2 = (localPosition2.y = y2);
			Vector3 vector4 = (transform.localPosition = localPosition2);
		}
	}

	public virtual void Main()
	{
	}
}
