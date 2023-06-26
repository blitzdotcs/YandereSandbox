using System;
using UnityEngine;

[Serializable]
public class GateScript : MonoBehaviour
{
	public ClockScript Clock;

	public Transform GateCollider;

	public Transform RightGate;

	public Transform LeftGate;

	public bool UpdateGates;

	public bool Closed;

	public virtual void Update()
	{
		if (!(Clock.PresentTime / 60f <= 8.5f) && !(Clock.PresentTime / 60f >= 15.5f))
		{
			Closed = true;
		}
		else
		{
			Closed = false;
		}
		if (!Closed)
		{
			GateCollider.localScale = Vector3.Lerp(GateCollider.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 10f);
			float x = Mathf.Lerp(RightGate.localPosition.x, 7f, Time.deltaTime);
			Vector3 localPosition = RightGate.localPosition;
			float num = (localPosition.x = x);
			Vector3 vector2 = (RightGate.localPosition = localPosition);
			float x2 = Mathf.Lerp(LeftGate.localPosition.x, -7f, Time.deltaTime);
			Vector3 localPosition2 = LeftGate.localPosition;
			float num2 = (localPosition2.x = x2);
			Vector3 vector4 = (LeftGate.localPosition = localPosition2);
		}
		else
		{
			GateCollider.localScale = Vector3.Lerp(GateCollider.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
			float x3 = Mathf.Lerp(RightGate.localPosition.x, 2.325f, Time.deltaTime);
			Vector3 localPosition3 = RightGate.localPosition;
			float num3 = (localPosition3.x = x3);
			Vector3 vector6 = (RightGate.localPosition = localPosition3);
			float x4 = Mathf.Lerp(LeftGate.localPosition.x, -2.325f, Time.deltaTime);
			Vector3 localPosition4 = LeftGate.localPosition;
			float num4 = (localPosition4.x = x4);
			Vector3 vector8 = (LeftGate.localPosition = localPosition4);
		}
	}

	public virtual void Main()
	{
	}
}
